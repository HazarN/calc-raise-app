using CalcRaise.Application.Abstractions;
using CalcRaise.Domain.Entities;

namespace CalcRaise.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CalcRaiseDbContext _context;

    public UnitOfWork(CalcRaiseDbContext context)
    {
        _context = context;
        MuscleGroups = new Repository<MuscleGroup>(context);
        Methods = new Repository<Method>(context);
        Exercises = new Repository<Exercise>(context);
        ProgramDays = new Repository<ProgramDay>(context);
        ProgramExercises = new Repository<ProgramExercise>(context);
        WorkoutWeeks = new Repository<WorkoutWeek>(context);
        WorkoutSetLogs = new Repository<WorkoutSetLog>(context);
    }

    public IRepository<MuscleGroup> MuscleGroups { get; }
    public IRepository<Method> Methods { get; }
    public IRepository<Exercise> Exercises { get; }
    public IRepository<ProgramDay> ProgramDays { get; }
    public IRepository<ProgramExercise> ProgramExercises { get; }
    public IRepository<WorkoutWeek> WorkoutWeeks { get; }
    public IRepository<WorkoutSetLog> WorkoutSetLogs { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
