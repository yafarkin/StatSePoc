using DataService.HealthChecks;
using DataService.Impl.Api;
using DataService.Impl.Db;
using DataService.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataService;

public static class Registrar
{
    public static IServiceCollection AddDataService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDataService, Impl.Api.DataService>();
        services.AddScoped<ISampleDataService, SampleDataService>();

        services.AddHealthChecks()
            .AddCheck<DbHealthCheck>("db", tags: ["ready"]);

        services.AddSingleton<DbInitializer>();
        services.AddSingleton<IDbWarmupState, DbWarmupState>();

        services.AddHostedService<DbWarmupService>();

        var connectionString = configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Default' not found");
        }

        var builder = new SqliteConnectionStringBuilder(connectionString);

        if (!Path.IsPathRooted(builder.DataSource))
        {
            var basePath = AppContext.BaseDirectory;
            var fullPath = Path.Combine(basePath, builder.DataSource);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            builder.DataSource = fullPath;
        }

        services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(builder.ToString()));

        return services;
    }
}