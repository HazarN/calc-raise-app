using CalcRaise.Domain.Common;

namespace CalcRaise.Domain.Entities;

public class MuscleGroup : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
}
