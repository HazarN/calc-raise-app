using CalcRaise.Application.Abstractions;
using CalcRaise.Application.Services.VolumeLoad;
using CalcRaise.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CalcRaise.Application.WeeklyBoard;

public sealed class WeeklyBoardService : IWeeklyBoardService
{
    private readonly IApplicationReadContext _db;
    private readonly IVolumeLoadCalculator _volumeCalculator;

    public WeeklyBoardService(IApplicationReadContext db, IVolumeLoadCalculator volumeCalculator)
    {
        _db = db;
        _volumeCalculator = volumeCalculator;
    }

    public async Task<IReadOnlyList<WeeklyBoardRowDto>> GetBoardAsync(int programDayId, int currentWorkoutWeekId, CancellationToken ct = default)
    {
        var previousWeekId = await _db.WorkoutWeeks
            .Where(w => w.Id < currentWorkoutWeekId)
            .OrderByDescending(w => w.Id)
            .Select(w => (int?)w.Id)
            .FirstOrDefaultAsync(ct);

        var programExercises = await _db.ProgramExercises
            .Where(pe => pe.ProgramDayId == programDayId)
            .OrderBy(pe => pe.Order)
            .Select(pe => new
            {
                pe.Id,
                pe.ExerciseId,
                ExerciseName = pe.Exercise.Name,
                MethodName = pe.Exercise.Method != null ? pe.Exercise.Method.Name : null,
                PrimaryMuscleGroupName = pe.Exercise.MuscleGroups
                    .Where(mg => mg.Involvement == MuscleInvolvement.Primary)
                    .Select(mg => mg.MuscleGroup.Name)
                    .FirstOrDefault(),
                pe.TargetSetsMin,
                pe.TargetSetsMax,
                pe.TargetRepsMin,
                pe.TargetRepsMax,
            })
            .ToListAsync(ct);

        var rows = new List<WeeklyBoardRowDto>(programExercises.Count);
        foreach (var pe in programExercises)
        {
            var currentWeekSets = await LoadSetsAsync(pe.Id, currentWorkoutWeekId, ct);
            var previousWeekSets = previousWeekId is null
                ? Array.Empty<SetLogDto>()
                : await LoadSetsAsync(pe.Id, previousWeekId.Value, ct);
            var allTimePersonalRecord = await FindAllTimeRecordAsync(pe.ExerciseId, ct);

            rows.Add(new WeeklyBoardRowDto(
                pe.Id,
                pe.ExerciseName,
                pe.PrimaryMuscleGroupName ?? "-",
                pe.MethodName,
                pe.TargetSetsMin,
                pe.TargetSetsMax,
                pe.TargetRepsMin,
                pe.TargetRepsMax,
                currentWeekSets,
                previousWeekSets,
                allTimePersonalRecord));
        }

        return rows;
    }

    private async Task<IReadOnlyList<SetLogDto>> LoadSetsAsync(int programExerciseId, int workoutWeekId, CancellationToken ct) =>
        await _db.WorkoutSetLogs
            .Where(s => s.ProgramExerciseId == programExerciseId && s.WorkoutWeekId == workoutWeekId)
            .OrderBy(s => s.SetNumber)
            .Select(s => new SetLogDto(s.SetNumber, s.Reps, s.WeightKg))
            .ToListAsync(ct);

    /// <summary>
    /// All-time PR = the historical week (across every ProgramExercise placement of this Exercise)
    /// whose total volume load is the highest. See backlog.md decision #1.
    /// </summary>
    private async Task<WeeklyExerciseRecordDto?> FindAllTimeRecordAsync(int exerciseId, CancellationToken ct)
    {
        var history = await _db.WorkoutSetLogs
            .Where(s => s.ProgramExercise.ExerciseId == exerciseId)
            .Select(s => new { s.WorkoutWeekId, s.SetNumber, s.Reps, s.WeightKg })
            .ToListAsync(ct);

        if (history.Count == 0)
        {
            return null;
        }

        return history
            .GroupBy(s => s.WorkoutWeekId)
            .Select(week => new
            {
                WorkoutWeekId = week.Key,
                Sets = week.OrderBy(s => s.SetNumber).Select(s => new SetLogDto(s.SetNumber, s.Reps, s.WeightKg)).ToList(),
                Volume = _volumeCalculator.CalculateVolume(week.Select(s => new SetPerformance(s.Reps, s.WeightKg))),
            })
            .OrderByDescending(week => week.Volume)
            .Select(week => new WeeklyExerciseRecordDto(week.WorkoutWeekId, week.Volume, week.Sets))
            .First();
    }
}
