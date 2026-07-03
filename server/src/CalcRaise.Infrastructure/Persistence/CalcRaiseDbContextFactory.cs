using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CalcRaise.Infrastructure.Persistence;

/// <summary>
/// Used only by `dotnet ef migrations add/remove` at design time, so migration files can be
/// generated without a real (Neon) connection string being configured. Never used at runtime —
/// the Api project wires up the real connection string via DependencyInjection.AddInfrastructure.
/// </summary>
public sealed class CalcRaiseDbContextFactory : IDesignTimeDbContextFactory<CalcRaiseDbContext>
{
    public CalcRaiseDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CalcRaiseDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=calcraise_design;Username=design;Password=design");
        return new CalcRaiseDbContext(optionsBuilder.Options);
    }
}
