using DataService.Entities;

namespace DataService.Interfaces.Repositories;

public interface IScriptRepository
{
    Task<IEnumerable<ScriptEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ScriptEntity?> GetAsync(string tag, string name, CancellationToken cancellationToken = default);
    Task<long> CreateAsync(ScriptEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ScriptEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}