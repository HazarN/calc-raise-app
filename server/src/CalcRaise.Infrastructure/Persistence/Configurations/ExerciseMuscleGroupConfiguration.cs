using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class ExerciseMuscleGroupConfiguration : IEntityTypeConfiguration<ExerciseMuscleGroup>
{
    public void Configure(EntityTypeBuilder<ExerciseMuscleGroup> builder)
    {
        builder.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });

        builder.Property(x => x.InvolvementPercentage).HasPrecision(5, 2);

        builder.HasOne(x => x.Exercise)
            .WithMany(e => e.MuscleGroups)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MuscleGroup)
            .WithMany(m => m.ExerciseMuscleGroups)
            .HasForeignKey(x => x.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
