namespace CalcRaise.Application.ProgressiveOverload;

public interface IProgressiveOverloadService
{
    /// <summary>
    /// Compares <paramref name="currentWorkoutWeekId"/>'s volume load against the average of the
    /// preceding <paramref name="lookbackWeeks"/> weeks, overall and per (primary) muscle group.
    /// See warehouse/backlog.md decision #5.
    /// </summary>
    Task<ProgressiveOverloadReportDto> GetReportAsync(int currentWorkoutWeekId, int lookbackWeeks = 4, CancellationToken ct = default);
}
