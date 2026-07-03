namespace CalcRaise.Application.ProgressiveOverload;

public sealed record OverallProgressDto(
    decimal CurrentWeekVolume,
    decimal? AverageOfPriorWeeksVolume,
    decimal? ProgressPercentage,
    int PositiveStreakWeeks);

public sealed record MuscleGroupProgressDto(
    int MuscleGroupId,
    string MuscleGroupName,
    decimal CurrentWeekVolume,
    decimal? AverageOfPriorWeeksVolume,
    decimal? ProgressPercentage,
    int PositiveStreakWeeks);

public sealed record ProgressiveOverloadReportDto(
    OverallProgressDto Overall,
    IReadOnlyList<MuscleGroupProgressDto> MuscleGroups);
