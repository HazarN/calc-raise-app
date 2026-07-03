using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CalcRaise.Infrastructure.Persistence;

/// <summary>
/// Used by `dotnet ef migrations add/remove` and `dotnet ef database update` at design time.
/// Reads the same user-secret (ConnectionStrings:CalcRaise, under the Api project's
/// UserSecretsId) the running Api uses, so `database update` targets the real DB — but falls
/// back to a dummy connection string when no secret is configured, so `migrations add` still
/// works on a machine with no Neon credentials set up yet. Never used at runtime.
/// </summary>
public sealed class CalcRaiseDbContextFactory : IDesignTimeDbContextFactory<CalcRaiseDbContext>
{
    private const string ApiUserSecretsId = "caf5d4df-bf0f-40f4-96d1-10aa21d84fa8";

    public CalcRaiseDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(ApiUserSecretsId)
            .Build();

        var connectionString = configuration.GetConnectionString("CalcRaise")
            ?? "Host=localhost;Database=calcraise_design;Username=design;Password=design";

        var optionsBuilder = new DbContextOptionsBuilder<CalcRaiseDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new CalcRaiseDbContext(optionsBuilder.Options);
    }
}
