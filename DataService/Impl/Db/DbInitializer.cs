using System.Data;
using Bogus;
using Dapper;
using DataService.Entities;
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

        var random = new Random();

        var tags = new[] { "click", "view", "purchase", "login", "error" };
        var metricNames = new[] { "metric1", "metric2", "metric3", "metric4", "metric5", "metric6" };

        var startDate = DateTimeOffset.UtcNow.AddMonths(-1);
        var endDate = DateTimeOffset.UtcNow;

        var faker = new Faker<MetricValueEntity>()
            .RuleFor(x => x.UserId, _ => random.Next(1, 1000))
            .RuleFor(x => x.UserGroupId, f => null)
            .RuleFor(x => x.Tag, f => f.PickRandom(tags))
            .RuleFor(x => x.MetricName, f => f.PickRandom(metricNames))
            .RuleFor(x => x.Value, f => f.Random.Double(0, 100))
            .RuleFor(x => x.CreatedAt, f => DateOnly.FromDateTime(f.Date.BetweenOffset(startDate, endDate).DateTime).ToString("O"));

        const int total = 100_000;
        const int batchSize = 1000;

        conn.Open();
        using var tx = conn.BeginTransaction();

        for (var i = 0; i < total; i += batchSize)
        {
            var batch = Enumerable.Range(0, batchSize)
                .Select(_ => faker.Generate())
                .ToList();

            var sql = @"
INSERT INTO MetricValue (
  Tag,
  UserId,
  UserGroupId,
  MetricName,
  Value,
  CreatedAt
)
VALUES (
  @Tag,
  @UserId,
  @UserGroupId,
  @MetricName,
  @Value,
  @CreatedAt
)
ON CONFLICT(Tag, UserId, MetricName, CreatedAt)
DO UPDATE SET
  Value = excluded.Value
";

            var cd = new CommandDefinition(
                commandText: sql,
                parameters: batch,
                transaction: tx,
                cancellationToken: cancellationToken);

            await conn.ExecuteAsync(cd);
        }
        
        tx.Commit();
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
    Tag TEXT NOT NULL,
    UserId INTEGER NOT NULL,
    MetricName TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UserGroupId TEXT NULL,
    Value REAL NOT NULL,
    PRIMARY KEY (Tag, UserId, MetricName, CreatedAt)   
);
", cancellationToken: cancellationToken));
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