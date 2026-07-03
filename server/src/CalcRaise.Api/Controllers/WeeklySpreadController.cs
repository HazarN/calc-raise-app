using CalcRaise.Application.WeeklySpread;
using Microsoft.AspNetCore.Mvc;

namespace CalcRaise.Api.Controllers;

[ApiController]
[Route("api/workout-weeks/{workoutWeekId:int}/weekly-spread")]
public sealed class WeeklySpreadController : ControllerBase
{
    private readonly IWeeklySpreadService _weeklySpreadService;

    public WeeklySpreadController(IWeeklySpreadService weeklySpreadService)
    {
        _weeklySpreadService = weeklySpreadService;
    }

    [HttpGet]
    public async Task<ActionResult<WeeklySpreadReportDto>> Get(int workoutWeekId, CancellationToken ct)
    {
        var report = await _weeklySpreadService.GetReportAsync(workoutWeekId, ct);
        return Ok(report);
    }
}
