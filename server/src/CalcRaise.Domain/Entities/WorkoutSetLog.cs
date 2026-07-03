using CalcRaise.Domain.Common;

namespace CalcRaise.Domain.Entities;

/// <summary>
/// Immutable log of a single performed set. "This week" / "previous week" / "all-time PR" are all
/// derived (queried) from this table rather than stored as separate physical columns.
/// </summary>
public class WorkoutSetLog : BaseEntity
{
    public int ProgramExerciseId { get; set; }
    public ProgramExercise ProgramExercise { get; set; } = null!;

    public int WorkoutWeekId { get; set; }
    public WorkoutWeek WorkoutWeek { get; set; } = null!;

    public int SetNumber { get; set; }
    public int Reps { get; set; }
    public decimal WeightKg { get; set; }

    /// <summary>Reps-in-reserve. Optional, informational only — not yet factored into PR/volume math (see backlog decision #2).</summary>
    public int? Rir { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
