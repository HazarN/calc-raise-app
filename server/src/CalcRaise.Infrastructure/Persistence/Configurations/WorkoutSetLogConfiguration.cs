using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class WorkoutSetLogConfiguration : IEntityTypeConfiguration<WorkoutSetLog>
{
    public void Configure(EntityTypeBuilder<WorkoutSetLog> builder)
    {
        builder.Property(x => x.WeightKg).HasPrecision(6, 2);

        builder.HasOne(x => x.ProgramExercise)
            .WithMany(pe => pe.SetLogs)
            .HasForeignKey(x => x.ProgramExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.WorkoutWeek)
            .WithMany(w => w.SetLogs)
            .HasForeignKey(x => x.WorkoutWeekId)
            .OnDelete(DeleteBehavior.Cascade);

        // Also the primary lookup path for the weekly board (current/previous week sets) and PR queries.
        builder.HasIndex(x => new { x.ProgramExerciseId, x.WorkoutWeekId, x.SetNumber }).IsUnique();
    }
}
