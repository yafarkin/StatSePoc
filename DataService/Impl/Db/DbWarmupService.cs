using DataService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataService.Impl.Db;

internal sealed class DbWarmupService : BackgroundService
{
    private readonly ILogger<DbWarmupService> _logger;
    private readonly IDbWarmupState _state;
    
    private readonly DbInitializer _initializer;

    public DbWarmupService(
        DbInitializer initializer,
        ILogger<DbWarmupService> logger,
        IDbWarmupState state)
    {
        _initializer = initializer;
        _logger = logger;
        _state = state;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Db warmup started");

            await _initializer.InitializeAsync(stoppingToken);

            _state.SetReady();

            _logger.LogInformation("Db warmup completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Db warmup failed");
            throw;
        }
    }
}