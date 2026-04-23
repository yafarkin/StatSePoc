using System.Data;
using Dapper;

namespace DataService.Impl.Db;

internal sealed partial class DbInitializer
{
    private async Task CreateMetricMetadataAsync(IDbConnection conn, CancellationToken cancellationToken)
    {
        await conn.ExecuteAsync(new CommandDefinition(@"
CREATE TABLE IF NOT EXISTS MetricMetadata (
    Tag TEXT NOT NULL,
    Key1 TEXT NOT NULL,
    Key2 TEXT NOT NULL,
    Key3 TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    ExpiredAfter TEXT,
    Data TEXT,
    PRIMARY KEY (Tag, Key1, Key2, Key3)   
);
", cancellationToken: cancellationToken));
    }
}