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

        await conn.ExecuteAsync("""
CREATE TABLE IF NOT EXISTS Scripts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Tag TEXT NOT NULL,
    Name TEXT NOT NULL,
    Content TEXT NOT NULL
);
""", cancellationToken);

        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Scripts", cancellationToken);

        if (count == 0)
        {
            await conn.ExecuteAsync("""
INSERT INTO Scripts (Tag, Name, Content)
VALUES (@Tag, @Name, @Content)
""",
                new[]
                {
                    new { Tag = "core", Name = "hello", Content = "return 'hello';" }
                });
        }
    }
}