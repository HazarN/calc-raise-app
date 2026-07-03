namespace CalcRaise.Application.Services.VolumeLoad;

/// <summary>One performed set, stripped of identity — the only inputs the PR/volume math needs.</summary>
public readonly record struct SetPerformance(int Reps, decimal WeightKg);
