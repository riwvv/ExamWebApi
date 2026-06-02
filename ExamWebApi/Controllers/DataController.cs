using ExamWebApi.Data;
using ExamWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(AppDbContext _context) : ControllerBase {
    [HttpGet("building")]
    public async Task<ActionResult<IEnumerable<BuildingResponseDto>>> GetBuildings() => Ok(await _context.Buildings.Include(x => x.Elevators)
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
                MoveSpeed = e.MoveSpeed,
                Status = e.Status
            }).ToList()
        })
        .AsSplitQuery()
        .ToListAsync());

    [HttpGet("elevator")]
    public async Task<ActionResult<IEnumerable<ElevatorAnaliticResponseDto>>> GetElevators() => Ok(await _context.Elevators
        .Include(x => x.Building)
        .Include(x => x.FloorCalls)
        .Include(x => x.TripLogs)
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
        .ToListAsync());
}
