using CalcRaise.Domain.Enums;

namespace CalcRaise.Domain.Entities;

/// <summary>Join entity: an exercise's involvement in a muscle group (primary/secondary + optional %).</summary>
public class ExerciseMuscleGroup
{
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int MuscleGroupId { get; set; }
    public MuscleGroup MuscleGroup { get; set; } = null!;

    public MuscleInvolvement Involvement { get; set; }
    public decimal? InvolvementPercentage { get; set; }
}
