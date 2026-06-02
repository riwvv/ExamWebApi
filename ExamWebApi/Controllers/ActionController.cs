using ExamWebApi.DTOs;
using ExamWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionController(SearchElevatorService _searchElevatorService) : ControllerBase {
    [HttpPost("call")]
    public async Task<ActionResult<CallResponseDto>> CallElevator([FromBody] CallRequestDto callRequest) {
        var result = await _searchElevatorService.SearchElevatorAsync(callRequest);
        return result == null ? BadRequest("Отказ сервиса") : Ok(result);
    }
}
