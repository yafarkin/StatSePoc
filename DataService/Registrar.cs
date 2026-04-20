using DataService.HealthChecks;
using DataService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataService;

public static class Registrar
{
    public static IServiceCollection AddDataService(this IServiceCollection services)
    {
        services.AddScoped<IDataService, Impl.DataService>();
        services.AddScoped<ISampleDataService, Impl.SampleDataService>();
        
        services.AddHealthChecks()
            .AddCheck<DbHealthCheck>("db", tags: ["ready"]);
        
        return services;
    }
}