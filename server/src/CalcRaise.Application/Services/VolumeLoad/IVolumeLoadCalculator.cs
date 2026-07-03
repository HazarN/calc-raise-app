namespace CalcRaise.Application.Services.VolumeLoad;

/// <summary>
/// PR/progressive-overload comparison metric (see warehouse/01-analiz-ve-degerlendirme.md §2 and
/// backlog.md decision #1): a week's "load" for an exercise is the sum of set x reps x weight
/// across all sets performed that week. Weeks are compared by this single number.
/// </summary>
public interface IVolumeLoadCalculator
{
    decimal CalculateVolume(IEnumerable<SetPerformance> sets);
}
