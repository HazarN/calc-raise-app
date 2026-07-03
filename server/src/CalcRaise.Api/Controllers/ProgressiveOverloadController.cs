using CalcRaise.Application.ProgressiveOverload;
using Microsoft.AspNetCore.Mvc;

namespace CalcRaise.Api.Controllers;

[ApiController]
[Route("api/progressive-overload")]
public sealed class ProgressiveOverloadController : ControllerBase
{
    private readonly IProgressiveOverloadService _progressiveOverloadService;

    public ProgressiveOverloadController(IProgressiveOverloadService progressiveOverloadService)
    {
        _progressiveOverloadService = progressiveOverloadService;
    }

    [HttpGet]
    public async Task<ActionResult<ProgressiveOverloadReportDto>> Get(
        [FromQuery] int workoutWeekId, [FromQuery] int lookbackWeeks, CancellationToken ct)
    {
        var effectiveLookback = lookbackWeeks > 0 ? lookbackWeeks : 4;
        var report = await _progressiveOverloadService.GetReportAsync(workoutWeekId, effectiveLookback, ct);
        return Ok(report);
    }
}
