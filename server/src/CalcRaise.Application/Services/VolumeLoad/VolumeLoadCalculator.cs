namespace CalcRaise.Application.Services.VolumeLoad;

public sealed class VolumeLoadCalculator : IVolumeLoadCalculator
{
    public decimal CalculateVolume(IEnumerable<SetPerformance> sets) =>
        sets.Sum(s => s.Reps * s.WeightKg);
}
