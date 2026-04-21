using DataService.Dtos.Queries;
using DataService.Entities;

namespace DataService.Interfaces.Repositories;

internal interface IMetricValueRepository
{
    IEnumerable<MetricValueEntity> Get(MetricValueQuery query);
    Task CreateAsync(MetricValueEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(MetricValueEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}