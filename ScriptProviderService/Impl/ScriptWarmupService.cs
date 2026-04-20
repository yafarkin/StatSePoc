using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScriptProviderService.Interfaces;

namespace ScriptProviderService.Impl;

internal sealed class ScriptWarmupService : BackgroundService
{
    private readonly ILogger<ScriptWarmupService> _logger;
    private readonly IScriptWarmupState _state;
    private readonly IScriptLoader _loader;

    public ScriptWarmupService(
        ILogger<ScriptWarmupService> logger,
        IScriptWarmupState state,
        IScriptLoader loader)
    {
        _logger = logger;
        _state = state;
        _loader = loader;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Script warmup started");

            _loader.LoadAll();

            _state.SetReady();

            _logger.LogInformation("Script warmup completed");
            
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Script warmup failed");
            throw;
        }
    }
}