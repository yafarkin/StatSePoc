using DataService.Dtos.Queries;
using DataService.Entities;

namespace DataService.Interfaces.Repositories;

internal interface IMetricEntityRepository
{
    IEnumerable<MetricEventEntity> Get(MetricValueQuery query);
    Ulid Create(MetricEventEntity entity);
}