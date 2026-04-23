using DataService.Dtos.Queries;
using DataService.Entities;

namespace DataService.Interfaces.Repositories;

internal interface IMetricEventRepository
{
    IEnumerable<MetricEventEntity> Get(MetricValueQuery query);
    Task<Ulid> CreateAsync(MetricEventEntity entity, CancellationToken cancellationToken = default);
}