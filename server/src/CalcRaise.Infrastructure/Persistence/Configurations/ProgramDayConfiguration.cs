using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class ProgramDayConfiguration : IEntityTypeConfiguration<ProgramDay>
{
    public void Configure(EntityTypeBuilder<ProgramDay> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(80);
        builder.HasIndex(x => x.DayOfWeek);
    }
}
