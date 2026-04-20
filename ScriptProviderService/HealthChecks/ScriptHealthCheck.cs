using Microsoft.Extensions.Diagnostics.HealthChecks;
using ScriptProviderService.Interfaces;

namespace ScriptProviderService.HealthChecks;

internal sealed class ScriptHealthCheck : IHealthCheck
{
    private readonly IScriptWarmupState _state;

    public ScriptHealthCheck(IScriptWarmupState state)
    {
        _state = state;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            _state.IsReady
                ? HealthCheckResult.Healthy("Scripts loaded")
                : HealthCheckResult.Unhealthy("Scripts still loading"));
    }
}