using CalcRaise.Application.Abstractions;
using CalcRaise.Application.Services.VolumeLoad;
using CalcRaise.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CalcRaise.Application.WeeklySpread;

public sealed class WeeklySpreadService : IWeeklySpreadService
{
    private readonly IApplicationReadContext _db;
    private readonly IVolumeLoadCalculator _volumeCalculator;

    public WeeklySpreadService(IApplicationReadContext db, IVolumeLoadCalculator volumeCalculator)
    {
        _db = db;
        _volumeCalculator = volumeCalculator;
    }

    public async Task<WeeklySpreadReportDto> GetReportAsync(int workoutWeekId, CancellationToken ct = default)
    {
        var plannedRowsRaw = await _db.ProgramExercises
            .Select(pe => new
            {
                pe.Id,
                pe.ProgramDay.DayOfWeek,
                ExerciseName = pe.Exercise.Name,
                MuscleGroupId = pe.Exercise.MuscleGroups
                    .Where(mg => mg.Involvement == MuscleInvolvement.Primary)
                    .Select(mg => (int?)mg.MuscleGroupId)
                    .FirstOrDefault(),
                MuscleGroupName = pe.Exercise.MuscleGroups
                    .Where(mg => mg.Involvement == MuscleInvolvement.Primary)
                    .Select(mg => mg.MuscleGroup.Name)
                    .FirstOrDefault(),
            })
            .ToListAsync(ct);

        var plannedRows = plannedRowsRaw
            .Where(r => r.MuscleGroupId != null)
            .Select(r => new PlannedRow(r.Id, r.DayOfWeek, r.ExerciseName, r.MuscleGroupId, r.MuscleGroupName))
            .ToList();

        var setLogsByProgramExercise = await _db.WorkoutSetLogs
            .Where(s => s.WorkoutWeekId == workoutWeekId)
            .Select(s => new { s.ProgramExerciseId, s.Reps, s.WeightKg })
            .ToListAsync(ct);
        var setLogLookup = setLogsByProgramExercise
            .GroupBy(s => s.ProgramExerciseId)
            .ToDictionary(g => g.Key, g => g.Select(s => new SetPerformance(s.Reps, s.WeightKg)).ToList());

        var cells = plannedRows
            .GroupBy(r => (r.DayOfWeek, r.MuscleGroupId!.Value, r.MuscleGroupName!))
            .Select(g =>
            {
                var performances = g
                    .SelectMany(r => setLogLookup.GetValueOrDefault(r.ProgramExerciseId) ?? [])
                    .ToList();

                return new WeeklySpreadCellDto(
                    g.Key.DayOfWeek,
                    g.Key.Item2,
                    g.Key.Item3,
                    g.Select(r => r.ExerciseName).Distinct().ToList(),
                    performances.Count,
                    _volumeCalculator.CalculateVolume(performances),
                    InsufficientRestWarning: false);
            })
            .ToList();

        return new WeeklySpreadReportDto(FlagInsufficientRest(cells));
    }

    /// <summary>
    /// Aynı kas grubu haftanın günleri arasında 48 saatten (2 günden) az arayla tekrar
    /// ediyorsa, ilgili günlerin ikisini de işaretler. Sadece o hafta gerçekten set
    /// girilmiş günler dikkate alınır (planlanıp yapılmayan gün yorgunluk oluşturmaz).
    /// Hafta sınırı ötesi (geçen haftanın Pazar'ı - bu haftanın Pazartesi'si gibi) v0'da
    /// kapsam dışı.
    /// </summary>
    private static List<WeeklySpreadCellDto> FlagInsufficientRest(List<WeeklySpreadCellDto> cells)
    {
        var warningKeys = new HashSet<(DayOfWeek, int)>();

        foreach (var group in cells.Where(c => c.TotalSets > 0).GroupBy(c => c.MuscleGroupId))
        {
            var trainedDays = group
                .Select(c => (Day: c.DayOfWeek, Ordinal: MondayFirstOrdinal(c.DayOfWeek)))
                .OrderBy(x => x.Ordinal)
                .ToList();

            for (var i = 1; i < trainedDays.Count; i++)
            {
                if (trainedDays[i].Ordinal - trainedDays[i - 1].Ordinal < 2)
                {
                    warningKeys.Add((trainedDays[i - 1].Day, group.Key));
                    warningKeys.Add((trainedDays[i].Day, group.Key));
                }
            }
        }

        return cells
            .Select(c => warningKeys.Contains((c.DayOfWeek, c.MuscleGroupId)) ? c with { InsufficientRestWarning = true } : c)
            .ToList();
    }

    private static int MondayFirstOrdinal(DayOfWeek day) => ((int)day + 6) % 7;

    private sealed record PlannedRow(int ProgramExerciseId, DayOfWeek DayOfWeek, string ExerciseName, int? MuscleGroupId, string? MuscleGroupName);
}
