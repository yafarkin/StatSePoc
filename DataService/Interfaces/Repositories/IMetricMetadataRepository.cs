using DataService.Dtos.Queries;
using DataService.Entities;

namespace DataService.Interfaces.Repositories;

internal interface IMetricMetadataRepository
{
    IEnumerable<MetricMetadataEntity> Get(MetricMetadataQuery query);
    void Upsert(MetricMetadataEntity entity);
    void Delete(string tag, string key1, string key2, string key3);
}