namespace CalcRaise.Application.WeeklyBoard;

public interface IWeeklyBoardService
{
    Task<IReadOnlyList<WeeklyBoardRowDto>> GetBoardAsync(int programDayId, int currentWorkoutWeekId, CancellationToken ct = default);
}
