using System.Data;
using Bogus;
using Dapper;
using DataService.Entities;

namespace DataService.Impl.Db;

internal sealed partial class DbInitializer
{
    private async Task CreateMetricEventTableAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        await conn.ExecuteAsync(new CommandDefinition(@"
CREATE TABLE IF NOT EXISTS MetricEvent (
    Id TEXT NOT NULL,
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
    
    private async Task SeedMetricEventTableAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM MetricEvent", cancellationToken);

        if (count != 0)
        {
            return;
        }

        var random = new Random();

        var tags = new[] { "click", "view", "purchase", "login", "error" };
        var metricNames = new[] { "metric1", "metric2", "metric3", "metric4", "metric5", "metric6" };

        var startDate = DateTimeOffset.UtcNow.AddMonths(-1);
        var endDate = DateTimeOffset.UtcNow;

        var faker = new Faker<MetricEventEntity>()
            .RuleFor(x => x.Id, _ => Ulid.NewUlid().ToString())
            .RuleFor(x => x.UserId, _ => random.Next(1, 1000))
            .RuleFor(x => x.UserGroupId, f => null)
            .RuleFor(x => x.Tag, f => f.PickRandom(tags))
            .RuleFor(x => x.MetricName, f => f.PickRandom(metricNames))
            .RuleFor(x => x.Value, f => f.Random.Double(0, 100))
            .RuleFor(x => x.CreatedAt, f => f.Date.BetweenOffset(startDate, endDate).ToString("O"));

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
INSERT INTO MetricEvent (
  Id,                       
  Tag,
  UserId,
  UserGroupId,
  MetricName,
  Value,
  CreatedAt
)
VALUES (
  @Id,      
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
    
}