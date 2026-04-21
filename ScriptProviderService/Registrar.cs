using Common.Impl;
using Microsoft.Extensions.DependencyInjection;
using ScriptProviderService.HealthChecks;
using ScriptProviderService.Impl;
using ScriptProviderService.Interfaces;

namespace ScriptProviderService;

public static class Registrar
{
    public static IServiceCollection AddScriptProviderService(this IServiceCollection services)
    {
        services.AddTransient<IScriptProvider, FileScriptProvider>();
        services.AddTransient<IScriptCatalog, FileScriptProvider>();
        
        services.AddSingleton<IScriptResolver, ScriptResolver>();
        services.AddSingleton<IScriptWarmupState, ScriptWarmupState>();
        services.AddSingleton<IScriptLoader, CacheScriptLoader>();

        services.AddHealthChecks()
            .AddCheck<ScriptHealthCheck>("scripts", tags: ["ready"]);
        
        services.AddHostedService<ScriptWarmupService>();        
   
        return services;
    }
}