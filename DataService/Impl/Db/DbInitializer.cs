using System.Data;
using Dapper;
using DataService.Interfaces;

namespace DataService.Impl.Db;

internal sealed class DbInitializer
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

        await SeedScriptTableAsync(conn, cancellationToken);
        await SeedMetricValueTableAsync(conn, cancellationToken);
    }

    private async Task SeedMetricValueTableAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM MetricValue", cancellationToken);

        if (count != 0)
        {
            return;
        }

        // await conn.ExecuteAsync("""
        //                         INSERT INTO Scripts (Tag, Name, Content)
        //                         VALUES (@Tag, @Name, @Content)
        //                         """,
        //     new[]
        //     {
        //         new { Tag = "core", Name = "hello", Content = "return 'hello';" }
        //     });
    }

    private async Task SeedScriptTableAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Scripts", cancellationToken);

        if (count != 0)
        {
            return;
        }

        await conn.ExecuteAsync(@"
INSERT INTO Scripts (Tag, Name, Content)
VALUES (@Tag, @Name, @Content)
", new[]
        {
            new { Tag = "core", Name = "hello", Content = "return 'hello';" }
        });
    }

    private async Task CreateMetricValueTableAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        await conn.ExecuteAsync(new CommandDefinition(@"
CREATE TABLE IF NOT EXISTS MetricValue (
    Id TEXT PRIMARY KEY,
    Tag TEXT NOT NULL,
    UserId INTEGER NOT NULL,
    UserIdGroup TEXT NULL,
    MetricName TEXT NOT NULL,
    Value REAL NOT NULL,
    CreatedAt TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_metric_tag_user_metric
    ON MetricValue(Tag, UserId, MetricName);

CREATE INDEX IF NOT EXISTS idx_metric_created_at
    ON MetricValue(CreatedAt);", cancellationToken: cancellationToken));
    }

    private async Task CreateScriptTableAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        await conn.ExecuteAsync(new CommandDefinition(@"
CREATE TABLE IF NOT EXISTS Scripts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Tag TEXT NOT NULL,
    Name TEXT NOT NULL,
    Content TEXT NOT NULL
);", cancellationToken: cancellationToken));
    }
}