using ExamWebApi.DTOs;
using ExamWebApi.Models;
using ExamWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreationController(CreateBuildingService _buildingService, CreateElevatorService _elevatorService) : ControllerBase {
    [HttpPost("building")]
    public async Task<ActionResult<Building>> CreateBuilding([FromBody] BuildingDto building) {
        var result = await _buildingService.CreateNewBuildingAsync(building);

        return result == null ? BadRequest("Не удалось добавить новое здание") : Ok(result);
    }

    [HttpPost("elevator")]
    public async Task<ActionResult<ElevatorAnaliticResponseDto>> CreateElevator([FromBody] ElevatorDto elevator) {
        var result = await _elevatorService.CreateNewElevatorAsync(elevator);

        return result == null ? BadRequest("Не удалось добавить новый лифт") : Ok(result);
    }
}
