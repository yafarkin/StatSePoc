using MetricService.Impl.Prometheus;
using MetricService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MetricService;

public static class Registrar
{
    public static IServiceCollection AddMetricsService(this IServiceCollection services)
    {
        services.AddSingleton<PrometheusMetrics>();
        
        services.AddSingleton<IScriptMetrics>(sp => sp.GetRequiredService<PrometheusMetrics>());
        services.AddSingleton<IMetricValueServiceMetrics>(sp => sp.GetRequiredService<PrometheusMetrics>());

        return services;
    }
}