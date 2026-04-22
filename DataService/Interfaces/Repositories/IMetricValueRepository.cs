using DataService.Dtos.Queries;
using DataService.Entities;

namespace DataService.Interfaces.Repositories;

internal interface IMetricValueRepository
{
    IEnumerable<MetricValueEntity> Get(MetricValueQuery query);
    Task UpsertAsync(MetricValueEntity entity, CancellationToken cancellationToken = default);
}