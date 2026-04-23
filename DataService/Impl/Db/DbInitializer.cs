using System.Data;
using Bogus;
using Dapper;
using DataService.Entities;
using DataService.Interfaces;

namespace DataService.Impl.Db;

internal sealed partial class DbInitializer
{
    private readonly IDbConnectionFactory _factory;

    public DbInitializer(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();

        await CreateScriptTableAsync(conn, cancellationToken);
        await CreateMetricValueTableAsync(conn, cancellationToken);
        await CreateMetricEventTableAsync(conn, cancellationToken);

        await SeedScriptTableAsync(conn, cancellationToken);
        await SeedMetricValueTableAsync(conn, cancellationToken);
        await SeedMetricEventTableAsync(conn, cancellationToken);
    }
}