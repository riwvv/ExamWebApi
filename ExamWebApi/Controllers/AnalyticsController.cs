using ExamWebApi.DTOs;
using ExamWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(AnalyticsDegradationService _analyticsDegradationService) : ControllerBase {
    [HttpGet("degradation")]
    public async Task<ActionResult<IEnumerable<ElevatorAnaliticResponseDto>>> GetDegradationElevators() {
        var result = await _analyticsDegradationService.GetDegradationElevatorsAsync();
        return result == null ? NotFound() : result.ToList();
    }
}
