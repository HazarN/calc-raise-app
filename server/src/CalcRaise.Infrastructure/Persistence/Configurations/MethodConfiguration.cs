using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class MethodConfiguration : IEntityTypeConfiguration<Method>
{
    public void Configure(EntityTypeBuilder<Method> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(80);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
