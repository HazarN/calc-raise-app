using CalcRaise.Application.Abstractions;
using CalcRaise.Application.Services.VolumeLoad;
using CalcRaise.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CalcRaise.Application.ProgressiveOverload;

public sealed class ProgressiveOverloadService : IProgressiveOverloadService
{
    private readonly IApplicationReadContext _db;
    private readonly IVolumeLoadCalculator _volumeCalculator;

    public ProgressiveOverloadService(IApplicationReadContext db, IVolumeLoadCalculator volumeCalculator)
    {
        _db = db;
        _volumeCalculator = volumeCalculator;
    }

    public async Task<ProgressiveOverloadReportDto> GetReportAsync(int currentWorkoutWeekId, int lookbackWeeks = 4, CancellationToken ct = default)
    {
        var orderedWeekIds = await _db.WorkoutWeeks
            .OrderBy(w => w.WeekStartDate)
            .Select(w => w.Id)
            .ToListAsync(ct);

        var currentIndex = orderedWeekIds.IndexOf(currentWorkoutWeekId);
        if (currentIndex < 0)
        {
            throw new InvalidOperationException($"WorkoutWeek {currentWorkoutWeekId} not found.");
        }

        var priorWeekIds = orderedWeekIds.Take(currentIndex).ToList();
        var lookbackWeekIds = priorWeekIds.Count <= lookbackWeeks
            ? priorWeekIds
            : priorWeekIds.Skip(priorWeekIds.Count - lookbackWeeks).ToList();

        var relevantWeekIds = orderedWeekIds.Take(currentIndex + 1).ToHashSet();
        var setLogs = await _db.WorkoutSetLogs
            .Where(s => relevantWeekIds.Contains(s.WorkoutWeekId))
            .Select(s => new SetLogRow(
                s.WorkoutWeekId,
                s.Reps,
                s.WeightKg,
                s.ProgramExercise.Exercise.MuscleGroups
                    .Where(mg => mg.Involvement == MuscleInvolvement.Primary)
                    .Select(mg => (int?)mg.MuscleGroupId)
                    .FirstOrDefault(),
                s.ProgramExercise.Exercise.MuscleGroups
                    .Where(mg => mg.Involvement == MuscleInvolvement.Primary)
                    .Select(mg => mg.MuscleGroup.Name)
                    .FirstOrDefault()))
            .ToListAsync(ct);

        var overallByWeek = setLogs
            .GroupBy(r => r.WorkoutWeekId)
            .ToDictionary(g => g.Key, VolumeOf);
        var overallSeries = orderedWeekIds.Take(currentIndex + 1).Select(id => overallByWeek.GetValueOrDefault(id)).ToList();
        var overallStats = BuildStats(
            currentVolume: overallByWeek.GetValueOrDefault(currentWorkoutWeekId),
            lookbackVolumes: lookbackWeekIds.Where(overallByWeek.ContainsKey).Select(id => overallByWeek[id]).ToList(),
            chronologicalSeries: overallSeries);

        var muscleGroups = setLogs
            .Where(r => r.MuscleGroupId is not null)
            .GroupBy(r => (r.MuscleGroupId!.Value, r.MuscleGroupName!))
            .Select(g =>
            {
                var byWeek = g.GroupBy(r => r.WorkoutWeekId).ToDictionary(wg => wg.Key, VolumeOf);
                var series = orderedWeekIds.Take(currentIndex + 1).Select(id => byWeek.GetValueOrDefault(id)).ToList();
                var stats = BuildStats(
                    currentVolume: byWeek.GetValueOrDefault(currentWorkoutWeekId),
                    lookbackVolumes: lookbackWeekIds.Where(byWeek.ContainsKey).Select(id => byWeek[id]).ToList(),
                    chronologicalSeries: series);

                return new MuscleGroupProgressDto(
                    g.Key.Item1,
                    g.Key.Item2,
                    stats.CurrentVolume,
                    stats.AverageOfPriorWeeksVolume,
                    stats.ProgressPercentage,
                    stats.PositiveStreakWeeks);
            })
            .OrderBy(m => m.MuscleGroupName)
            .ToList();

        var overall = new OverallProgressDto(
            overallStats.CurrentVolume,
            overallStats.AverageOfPriorWeeksVolume,
            overallStats.ProgressPercentage,
            overallStats.PositiveStreakWeeks);

        return new ProgressiveOverloadReportDto(overall, muscleGroups);
    }

    private decimal VolumeOf(IEnumerable<SetLogRow> rows) =>
        _volumeCalculator.CalculateVolume(rows.Select(r => new SetPerformance(r.Reps, r.WeightKg)));

    private static ProgressStats BuildStats(decimal currentVolume, IReadOnlyList<decimal> lookbackVolumes, IReadOnlyList<decimal> chronologicalSeries)
    {
        decimal? average = lookbackVolumes.Count > 0 ? lookbackVolumes.Average() : null;
        decimal? progressPercentage = average is > 0
            ? Math.Round((currentVolume - average.Value) / average.Value * 100m, 1)
            : null;
        var streak = CalculatePositiveStreak(chronologicalSeries);

        return new ProgressStats(currentVolume, average, progressPercentage, streak);
    }

    /// <summary>Consecutive weeks (walking back from the most recent) where volume strictly increased.</summary>
    private static int CalculatePositiveStreak(IReadOnlyList<decimal> chronologicalVolumes)
    {
        var streak = 0;
        for (var i = chronologicalVolumes.Count - 1; i > 0; i--)
        {
            if (chronologicalVolumes[i] > chronologicalVolumes[i - 1])
            {
                streak++;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    private sealed record SetLogRow(int WorkoutWeekId, int Reps, decimal WeightKg, int? MuscleGroupId, string? MuscleGroupName);

    private sealed record ProgressStats(decimal CurrentVolume, decimal? AverageOfPriorWeeksVolume, decimal? ProgressPercentage, int PositiveStreakWeeks);
}
