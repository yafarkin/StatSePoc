using Dapper;
using DataService.Entities;
using DataService.Interfaces;
using DataService.Interfaces.Repositories;

namespace DataService.Impl.Db.Repositories;

internal sealed class ScriptRepository : IScriptRepository
{
    private readonly IDbConnectionFactory _factory;

    public ScriptRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<ScriptEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();

        return await conn.QueryAsync<ScriptEntity>(
            "SELECT Id, Tag, Name, Content FROM Scripts");
    }

    public async Task<ScriptEntity?> GetAsync(string tag, string name, CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();

        return await conn.QueryFirstOrDefaultAsync<ScriptEntity>("""
                                                                 SELECT Id, Tag, Name, Content
                                                                 FROM Scripts
                                                                 WHERE Tag = @Tag AND Name = @Name
                                                                 """,
            new { Tag = tag, Name = name });
    }

    public async Task<long> CreateAsync(ScriptEntity script, CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();

        var id = await conn.ExecuteScalarAsync<long>("""
                                                     INSERT INTO Scripts (Tag, Name, Content)
                                                     VALUES (@Tag, @Name, @Content);
                                                     SELECT last_insert_rowid();
                                                     """, script);

        return id;
    }

    public async Task UpdateAsync(ScriptEntity script, CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();

        await conn.ExecuteAsync("""
                                UPDATE Scripts
                                SET Tag = @Tag,
                                    Name = @Name,
                                    Content = @Content
                                WHERE Id = @Id
                                """, script);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();

        await conn.ExecuteAsync("DELETE FROM Scripts WHERE Id = @Id", new { Id = id });
    }
}