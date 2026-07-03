using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
{
    public void Configure(EntityTypeBuilder<MuscleGroup> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(80);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
