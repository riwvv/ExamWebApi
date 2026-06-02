using ExamWebApi.Data;
using ExamWebApi.DTOs;
using ExamWebApi.Models;
using ExamWebApi.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace ExamWebApi.Services;

public class SearchElevatorService(AppDbContext _context, ILogger<SearchElevatorService> _logger) {
    public async Task<CallResponseDto?> SearchElevatorAsync(CallRequestDto dto) {
		try {
			if (dto.BuildingId == Guid.Empty || dto.FloorNumber <= 0 || string.IsNullOrWhiteSpace(dto.Direction))
				throw new Exception("Получены недопустимые значения");

			var building = await _context.Buildings
                .Include(x => x.Elevators)
                .Select(x => new BuildingResponseDto {
                    Id = x.Id,
                    Adress = x.Adress,
                    TotalFloors = x.TotalFloors,
                    Elevators = x.Elevators.Select(e => new ElevatorResponseDto {
                        SerialNumber = e.SerialNumber,
                        ModelID = e.ModelID,
                        ProductionDate = e.ProductionDate,
                        MinFloor = e.MinFloor,
                        MaxFloor = e.MaxFloor,
                        CurrentFloor = e.CurrentFloor,
                        MoveStatus = e.MoveStatus,
                        MoveSpeed = e.MoveSpeed,
                        Status = e.Status
                    }).ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == dto.BuildingId);

            if (building == null)
				throw new Exception("Искомого здания не существует");

			var newFloorCall = new FloorCall {
				Id = Guid.NewGuid(),
				CurrentFloor = dto.FloorNumber	
			};

			if (!building.Elevators.Any() || building.Elevators.FirstOrDefault(x => x.Status == "active") == null)
				throw new Exception($"В здании №{building.Id} нет подходящих лифтов");

			var elevatorsInBuilding = building.Elevators.Where(x => x.Status == "active").ToList();
            _logger.LogInformation($"Лифтов в здании найдено: {elevatorsInBuilding.Count}");

			if (MyWrapper.TryFind(elevatorsInBuilding, dto.FloorNumber, out ElevatorResponseDto? result) && result != null) {
                _logger.LogInformation("Найден лифт на нашем этаже");

                var response = new CallResponseDto {
                    ArrivalTime = TimeOnly.FromDateTime(DateTime.Now),
                    ElevatorId = result.SerialNumber
                };

                newFloorCall.Course = "stay";
                newFloorCall.Timestamp = DateTime.Now;
                newFloorCall.ElevatorId = result.SerialNumber;
                newFloorCall.Elevator = await _context.Elevators.FindAsync(result.SerialNumber);

                (await _context.Elevators.FindAsync(result.SerialNumber))!.CurrentFloor = dto.FloorNumber;
                await _context.FloorCalls.AddAsync(newFloorCall);
                await _context.SaveChangesAsync();

                return response;
            }

            if (MyWrapper.TryFindAbove(elevatorsInBuilding, dto.FloorNumber, out IEnumerable<ElevatorResponseDto>? above) && above != null) {
                _logger.LogInformation($"Найдены лифты выше: {above.Count()}");

                if (above.Count() == 1) {
                    _logger.LogInformation("Был найден один лифт");

                    var foundAbove = above.First();

                    var time = Math.Abs(dto.FloorNumber - foundAbove.CurrentFloor) * 3 / foundAbove.MoveSpeed;

                    _logger.LogInformation($"Ожидание началось: {time}мс");

                    await Task.Delay(Convert.ToInt32(Math.Round(time * 1000, 0)));

                    var response = new CallResponseDto {
                        ArrivalTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Convert.ToDouble(time))),
                        ElevatorId = foundAbove.SerialNumber
                    };

                    newFloorCall.Course = "down";
                    newFloorCall.Timestamp = DateTime.Now;
                    newFloorCall.ElevatorId = foundAbove.SerialNumber;
                    newFloorCall.Elevator = await _context.Elevators.FindAsync(foundAbove.SerialNumber);

                    (await _context.Elevators.FindAsync(foundAbove.SerialNumber))!.CurrentFloor = dto.FloorNumber;
                    await _context.FloorCalls.AddAsync(newFloorCall);
                    await _context.SaveChangesAsync();

                    return response;
                }
                else if (above.Count() > 1) {
                    var floors = above.Select(x => x.CurrentFloor).Distinct().OrderBy(x => x).ToList();
                    var difference = above.Select(x => Math.Abs(x.CurrentFloor - dto.FloorNumber)).Min();

                    _logger.LogInformation($"Разница между мной и лифтом: {difference}");

                    var nearest = above.FirstOrDefault(x => x.CurrentFloor == dto.FloorNumber + difference);

                    _logger.LogInformation($"Найден ближайший лифт сверху: {nearest!.ModelID}");

                    var time = Math.Abs(dto.FloorNumber - nearest.CurrentFloor) * 3 / nearest.MoveSpeed;

                    _logger.LogInformation($"Ожидание началось: {time}мс");

                    await Task.Delay(Convert.ToInt32(Math.Round(time * 1000, 0)));

                    var response = new CallResponseDto {
                        ArrivalTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Convert.ToDouble(time))),
                        ElevatorId = nearest.SerialNumber
                    };

                    newFloorCall.Course = "down";
                    newFloorCall.Timestamp = DateTime.Now;
                    newFloorCall.ElevatorId = nearest.SerialNumber;
                    newFloorCall.Elevator = await _context.Elevators.FindAsync(nearest.SerialNumber);

                    (await _context.Elevators.FindAsync(nearest.SerialNumber))!.CurrentFloor = dto.FloorNumber;
                    await _context.FloorCalls.AddAsync(newFloorCall);
                    await _context.SaveChangesAsync();

                    return response;
                }
            }

            _logger.LogInformation("Лифтов выше нет, ищем снизу");

            if (MyWrapper.TryFindBelow(elevatorsInBuilding, dto.FloorNumber, out IEnumerable<ElevatorResponseDto>? below) && below != null) {
                _logger.LogInformation($"Найдены лифты ниже: {below.Count()}");

                if (below.Count() == 1) {
                    _logger.LogInformation("Был найден один лифт");

                    var foundBelow = below.First();

                    var time = Math.Abs(dto.FloorNumber - foundBelow.CurrentFloor) * 3 / foundBelow.MoveSpeed;

                    _logger.LogInformation($"Ожидание началось: {time}мс");

                    await Task.Delay(Convert.ToInt32(Math.Round(time * 1000, 0)));

                    var response = new CallResponseDto {
                        ArrivalTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Convert.ToDouble(time))),
                        ElevatorId = below.First()!.SerialNumber
                    };

                    newFloorCall.Course = "up";
                    newFloorCall.Timestamp = DateTime.Now;
                    newFloorCall.ElevatorId = foundBelow.SerialNumber;
                    newFloorCall.Elevator = await _context.Elevators.FindAsync(foundBelow.SerialNumber);

                    (await _context.Elevators.FindAsync(foundBelow.SerialNumber))!.CurrentFloor = dto.FloorNumber;
                    await _context.FloorCalls.AddAsync(newFloorCall);
                    await _context.SaveChangesAsync();

                    return response;
                }
                else if (below.Count() > 1) {
                    var floors = below.Select(x => x.CurrentFloor).Distinct().OrderBy(x => x).ToList();
                    var difference = below.Select(x => Math.Abs(x.CurrentFloor - dto.FloorNumber)).Min();

                    _logger.LogInformation($"Разница между мной и лифтом: {difference}");

                    var nearest = below.FirstOrDefault(x => x.CurrentFloor == dto.FloorNumber - difference);

                    _logger.LogInformation($"Найден ближайший лифт снизу: {nearest!.ModelID}");

                    var time = Math.Abs(dto.FloorNumber - nearest.CurrentFloor) * 3 / nearest.MoveSpeed;

                    _logger.LogInformation($"Ожидание началось: {time}мс");

                    await Task.Delay(Convert.ToInt32(Math.Round(time * 1000, 0)));

                    var response = new CallResponseDto {
                        ArrivalTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Convert.ToDouble(time))),
                        ElevatorId = nearest.SerialNumber
                    };

                    newFloorCall.Course = "up";
                    newFloorCall.Timestamp = DateTime.Now;
                    newFloorCall.ElevatorId = nearest.SerialNumber;
                    newFloorCall.Elevator = await _context.Elevators.FindAsync(nearest.SerialNumber);

                    (await _context.Elevators.FindAsync(nearest.SerialNumber))!.CurrentFloor = dto.FloorNumber;
                    await _context.FloorCalls.AddAsync(newFloorCall);
                    await _context.SaveChangesAsync();

                    return response;
                }
            }

            return null;
		}
		catch (Exception ex) {
			_logger.LogError(ex, "Ошибка поиска лифта");
			return null;
		}
    }
}
