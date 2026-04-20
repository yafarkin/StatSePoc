using DataService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataService;

public static class Registrar
{
    public static IServiceCollection AddDataService(this IServiceCollection services)
    {
        services.AddScoped<IDataService, Impl.DataService>();
        services.AddScoped<ISampleDataService, Impl.SampleDataService>();

        return services;
    }
}