using CalcRaise.Domain.Entities;

namespace CalcRaise.Application.Abstractions;

public interface IUnitOfWork
{
    IRepository<MuscleGroup> MuscleGroups { get; }
    IRepository<Method> Methods { get; }
    IRepository<Exercise> Exercises { get; }
    IRepository<ProgramDay> ProgramDays { get; }
    IRepository<ProgramExercise> ProgramExercises { get; }
    IRepository<WorkoutWeek> WorkoutWeeks { get; }
    IRepository<WorkoutSetLog> WorkoutSetLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
