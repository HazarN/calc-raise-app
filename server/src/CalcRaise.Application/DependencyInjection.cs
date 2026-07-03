using CalcRaise.Application.ProgressiveOverload;
using CalcRaise.Application.Services.VolumeLoad;
using CalcRaise.Application.WeeklyBoard;
using CalcRaise.Application.WeeklySpread;
using Microsoft.Extensions.DependencyInjection;

namespace CalcRaise.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IVolumeLoadCalculator, VolumeLoadCalculator>();
        services.AddScoped<IWeeklyBoardService, WeeklyBoardService>();
        services.AddScoped<IProgressiveOverloadService, ProgressiveOverloadService>();
        services.AddScoped<IWeeklySpreadService, WeeklySpreadService>();
        return services;
    }
}
