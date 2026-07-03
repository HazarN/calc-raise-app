using CalcRaise.Application.WeeklyBoard;
using Microsoft.AspNetCore.Mvc;

namespace CalcRaise.Api.Controllers;

[ApiController]
[Route("api/program-days/{programDayId:int}/weekly-board")]
public sealed class WeeklyBoardController : ControllerBase
{
    private readonly IWeeklyBoardService _weeklyBoardService;

    public WeeklyBoardController(IWeeklyBoardService weeklyBoardService)
    {
        _weeklyBoardService = weeklyBoardService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WeeklyBoardRowDto>>> Get(
        int programDayId, [FromQuery] int workoutWeekId, CancellationToken ct)
    {
        var board = await _weeklyBoardService.GetBoardAsync(programDayId, workoutWeekId, ct);
        return Ok(board);
    }
}
