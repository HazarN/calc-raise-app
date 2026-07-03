namespace CalcRaise.Application.WeeklySpread;

/// <summary>One (gün, kas grubu) hücresi. DayOfWeek, System.DayOfWeek'in ham int değeridir (Sunday=0).</summary>
public sealed record WeeklySpreadCellDto(
    DayOfWeek DayOfWeek,
    int MuscleGroupId,
    string MuscleGroupName,
    IReadOnlyList<string> ExerciseNames,
    int TotalSets,
    decimal TotalVolume,
    bool InsufficientRestWarning);

public sealed record WeeklySpreadReportDto(IReadOnlyList<WeeklySpreadCellDto> Cells);
