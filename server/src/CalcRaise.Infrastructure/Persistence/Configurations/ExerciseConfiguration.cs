using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(120);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasOne(x => x.Method)
            .WithMany()
            .HasForeignKey(x => x.MethodId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
