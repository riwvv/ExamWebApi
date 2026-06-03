using ExamWebApi.DTOs;
using ExamWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionController(SearchElevatorService _searchElevatorService, ActionElevatorService _actionElevatorService) : ControllerBase {
    [HttpPost("call")]
    public async Task<ActionResult<CallResponseDto>> CallElevator([FromBody] CallRequestDto callRequest) {
        var result = await _searchElevatorService.SearchElevatorAsync(callRequest);
        return result == null ? BadRequest("Отказ сервиса") : Ok(result);
    }

    [HttpPost("elevator/{id}/action")]
    public async Task<ActionResult<string>> StartElevator(Guid id, int targetFloor) {
        var result = await _actionElevatorService.StartElevatorAsync(id, targetFloor);
        return string.IsNullOrEmpty(result) ? BadRequest("Отмена поездки") : Ok(result);
    }
}
