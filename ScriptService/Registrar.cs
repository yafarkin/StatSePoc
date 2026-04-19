using Microsoft.Extensions.DependencyInjection;
using ScriptService.Impl;
using ScriptService.Interfaces;

namespace ScriptService;

public static class Registrar
{
    public static IServiceCollection AddScriptService(this IServiceCollection services)
    {
        services.AddScoped<IScriptExecutor, ScriptExecutor>();

        return services;
    }
}