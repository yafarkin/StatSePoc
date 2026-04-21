using Dapper;
using DataService.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DataService.HealthChecks;

internal sealed class DbHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _dbFactory;

    public DbHealthCheck(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            using var conn = _dbFactory.Create();
            
            var result = await conn.ExecuteScalarAsync<int>(
                new CommandDefinition("SELECT 1", cancellationToken));

            return result == 1
                ? HealthCheckResult.Healthy("DB OK")
                : HealthCheckResult.Unhealthy("DB returned unexpected result");        
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy("DB unavailable", e);
        }
    }
}