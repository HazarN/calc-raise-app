using CalcRaise.Domain.Common;

namespace CalcRaise.Domain.Entities;

/// <summary>An exercise placed on a specific program day, with its target set/rep range.</summary>
public class ProgramExercise : BaseEntity
{
    public int ProgramDayId { get; set; }
    public ProgramDay ProgramDay { get; set; } = null!;

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int Order { get; set; }

    public int TargetSetsMin { get; set; }
    public int TargetSetsMax { get; set; }
    public int TargetRepsMin { get; set; }
    public int TargetRepsMax { get; set; }

    public ICollection<WorkoutSetLog> SetLogs { get; set; } = new List<WorkoutSetLog>();
}
