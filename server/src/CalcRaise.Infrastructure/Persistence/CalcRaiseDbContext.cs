using CalcRaise.Application.Abstractions;
using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalcRaise.Infrastructure.Persistence;

public sealed class CalcRaiseDbContext : DbContext, IApplicationReadContext
{
    public CalcRaiseDbContext(DbContextOptions<CalcRaiseDbContext> options) : base(options)
    {
    }

    public DbSet<MuscleGroup> MuscleGroups => Set<MuscleGroup>();
    public DbSet<Method> Methods => Set<Method>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroups => Set<ExerciseMuscleGroup>();
    public DbSet<ProgramDay> ProgramDays => Set<ProgramDay>();
    public DbSet<ProgramExercise> ProgramExercises => Set<ProgramExercise>();
    public DbSet<WorkoutWeek> WorkoutWeeks => Set<WorkoutWeek>();
    public DbSet<WorkoutSetLog> WorkoutSetLogs => Set<WorkoutSetLog>();

    IQueryable<Exercise> IApplicationReadContext.Exercises => Exercises;
    IQueryable<ProgramDay> IApplicationReadContext.ProgramDays => ProgramDays;
    IQueryable<ProgramExercise> IApplicationReadContext.ProgramExercises => ProgramExercises;
    IQueryable<WorkoutWeek> IApplicationReadContext.WorkoutWeeks => WorkoutWeeks;
    IQueryable<WorkoutSetLog> IApplicationReadContext.WorkoutSetLogs => WorkoutSetLogs;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CalcRaiseDbContext).Assembly);
    }
}
