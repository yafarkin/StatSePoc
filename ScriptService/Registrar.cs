using Microsoft.Extensions.DependencyInjection;
using ScriptService.Handlers.RunScript;
using ScriptService.Impl;
using ScriptService.Interfaces;

namespace ScriptService;

public static class Registrar
{
    public static IServiceCollection AddScriptService(this IServiceCollection services)
    {
        services.AddScoped<IScriptExecutor, ScriptExecutor>();
        
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RunScriptQueryHandler).Assembly));        

        return services;
    }
}