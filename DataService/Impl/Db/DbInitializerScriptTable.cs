using System.Data;
using Dapper;

namespace DataService.Impl.Db;

internal sealed partial class DbInitializer
{
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
}