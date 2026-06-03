using ExamWebApi.Data;
using ExamWebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExamWebApi.Services;

public class AnalyticsDegradationService(AppDbContext _context, ILogger<AnalyticsDegradationService> _logger) {
    public async Task<IEnumerable<ElevatorAnaliticResponseDto>?> GetDegradationElevatorsAsync() {
        _logger.LogInformation("Начало поиска лифтов с риском поломки");
		try {
            var targerDate = DateTime.Now.AddDays(-30);
            var elevatorIds = await _context.TripLogs
                .Where(t => t.Timestamp >= targerDate)
                .GroupBy(t => t.ElevatorId)
                .Select(g => new {
                    ElevatorId = g.Key,
                    TotalDistance = g.Select(t => t.DistanceTraveled).Distinct().Sum()
                })
                .Where(x => x.TotalDistance >= 100000m)
                .Select(x => x.ElevatorId)
                .ToListAsync();

            var elevators = await _context.Elevators
                .Include(x => x.TripLogs)
                .Where(x => x.ProductionDate <= targerDate)
                .Where(x => elevatorIds.Contains(x.SerialNumber))
                .Select(x => new ElevatorAnaliticResponseDto {
                    SerialNumber = x.SerialNumber,
                    ModelID = x.ModelID,
                    ProductionDate = x.ProductionDate,
                    MinFloor = x.MinFloor,
                    MaxFloor = x.MaxFloor,
                    CurrentFloor = x.CurrentFloor,
                    MoveStatus = x.MoveStatus,
                    MoveSpeed = x.MoveSpeed,
                    Mileage = x.Mileage,
                    Status = x.Status,
                    Building = new BuildingDto {
                        Adress = x.Building.Adress,
                        TotalFloors = x.Building.TotalFloors
                    },
                    FloorCalls = x.FloorCalls.Select(f => new FloorCallDto {
                        Id = f.Id,
                        CurrentFloor = f.CurrentFloor,
                        Course = f.Course,
                        Timestamp = f.Timestamp
                    }).ToList(),
                    TripLogs = x.TripLogs.Select(t => new TripLogDto {
                        Id = t.Id,
                        Description = t.Description,
                        DistanceTraveled = t.DistanceTraveled,
                        TotalSeconds = t.TotalSeconds,
                        Timestamp = t.Timestamp
                    }).ToList()
                })
                .AsSplitQuery()
                .ToListAsync();

            if (elevators == null) {
                _logger.LogInformation("Лифтов с риском поломки не найдено");
                return null;
            }
            _logger.LogInformation($"Найдено: {elevators.Count}");
            return elevators;
		}
		catch (Exception ex) {
			_logger.LogError(ex, "Ошибка поиска лифтов с риском поломки");
			return null;
		}
    }
}
