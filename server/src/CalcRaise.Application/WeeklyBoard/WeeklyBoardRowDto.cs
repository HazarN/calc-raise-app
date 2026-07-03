namespace CalcRaise.Application.WeeklyBoard;

/// <summary>One row of the main weekly table: Hareket Bölgesi / Egzersiz / Method / hedef aralık / bu hafta / geçen hafta / all-time PR.</summary>
public sealed record WeeklyBoardRowDto(
    int ProgramExerciseId,
    string ExerciseName,
    string PrimaryMuscleGroupName,
    string? MethodName,
    int TargetSetsMin,
    int TargetSetsMax,
    int TargetRepsMin,
    int TargetRepsMax,
    IReadOnlyList<SetLogDto> CurrentWeekSets,
    IReadOnlyList<SetLogDto> PreviousWeekSets,
    WeeklyExerciseRecordDto? AllTimePersonalRecord);
