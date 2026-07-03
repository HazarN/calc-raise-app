using CalcRaise.Application.Abstractions;
using CalcRaise.Infrastructure.Persistence;
using CalcRaise.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CalcRaise.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CalcRaiseDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IApplicationReadContext>(sp => sp.GetRequiredService<CalcRaiseDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
