using ExamWebApi.Data;
using ExamWebApi.DTOs;
using ExamWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(AppDbContext _context) : ControllerBase {
    [HttpGet("building")]
    public async Task<ActionResult<IEnumerable<Building>>> GetBuildings() {
        var buildings = await _context.Buildings
        .Include(x => x.Elevators)
        .Select(b => new BuildingResponseDto {
            Adress = b.Adress,
            TotalFloors = b.TotalFloors,
            Elevators = b.Elevators.Select(e => new ElevatorResponseDto {
                SerialNumber = e.SerialNumber,
                ModelID = e.ModelID,
                ProductionDate = e.ProductionDate,
                MinFloor = e.MinFloor,
                MaxFloor = e.MaxFloor,
                MoveSpeed = e.MoveSpeed,
                Status = e.Status
            }).ToList()
        })
        .ToListAsync();

        return Ok(buildings);
    }

    [HttpGet("elevator")]
    public async Task<ActionResult<IEnumerable<Building>>> GetElevators() => Ok(await _context.Elevators.Include(x => x.FloorCalls).Include(x => x.TripLogs).ToListAsync());
}
