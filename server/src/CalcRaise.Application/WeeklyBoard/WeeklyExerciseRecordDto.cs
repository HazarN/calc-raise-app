namespace CalcRaise.Application.WeeklyBoard;

/// <summary>The all-time-PR week for an exercise: which week it was, its total volume, and the set/rep breakdown.</summary>
public sealed record WeeklyExerciseRecordDto(int WorkoutWeekId, decimal TotalVolume, IReadOnlyList<SetLogDto> Sets);
