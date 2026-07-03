using CalcRaise.Domain.Entities;

namespace CalcRaise.Application.Abstractions;

/// <summary>
/// Read-only, LINQ-queryable view of persistence for cross-aggregate reporting queries
/// (e.g. the weekly board) where the generic repository pattern would be awkward.
/// Writes always go through <see cref="IUnitOfWork"/> / <see cref="IRepository{TEntity}"/>.
/// </summary>
public interface IApplicationReadContext
{
    IQueryable<Exercise> Exercises { get; }
    IQueryable<ProgramDay> ProgramDays { get; }
    IQueryable<ProgramExercise> ProgramExercises { get; }
    IQueryable<WorkoutWeek> WorkoutWeeks { get; }
    IQueryable<WorkoutSetLog> WorkoutSetLogs { get; }
}
