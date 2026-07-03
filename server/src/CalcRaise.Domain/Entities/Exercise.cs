using CalcRaise.Domain.Common;

namespace CalcRaise.Domain.Entities;

public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int? MethodId { get; set; }
    public Method? Method { get; set; }

    public ICollection<ExerciseMuscleGroup> MuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
}
