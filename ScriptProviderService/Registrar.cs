using Microsoft.Extensions.DependencyInjection;
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

        services.AddSingleton<IScriptLoader, CacheScriptLoader>();

        return services;
    }
}