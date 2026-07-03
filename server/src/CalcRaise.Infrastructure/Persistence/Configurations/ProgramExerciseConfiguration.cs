using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class ProgramExerciseConfiguration : IEntityTypeConfiguration<ProgramExercise>
{
    public void Configure(EntityTypeBuilder<ProgramExercise> builder)
    {
        builder.HasOne(x => x.ProgramDay)
            .WithMany(d => d.ProgramExercises)
            .HasForeignKey(x => x.ProgramDayId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Exercise)
            .WithMany()
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ProgramDayId, x.Order });
    }
}
