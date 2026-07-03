using CalcRaise.Domain.Common;

namespace CalcRaise.Domain.Entities;

public class ProgramDay : BaseEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public string? Name { get; set; }

    public ICollection<ProgramExercise> ProgramExercises { get; set; } = new List<ProgramExercise>();
}
