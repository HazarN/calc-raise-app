using CalcRaise.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalcRaise.Infrastructure.Persistence.Configurations;

public sealed class WorkoutWeekConfiguration : IEntityTypeConfiguration<WorkoutWeek>
{
    public void Configure(EntityTypeBuilder<WorkoutWeek> builder)
    {
        builder.HasIndex(x => x.WeekStartDate).IsUnique();
    }
}
