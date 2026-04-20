using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DataService.HealthChecks;

internal sealed class DbHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}