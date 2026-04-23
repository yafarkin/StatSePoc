using Dapper;
using DataService.Dtos.Queries;
using DataService.Entities;
using DataService.Interfaces;
using DataService.Interfaces.Repositories;

namespace DataService.Impl.Db.Repositories;

internal sealed class MetricMetadataRepository : IMetricMetadataRepository
{
    private readonly IDbConnectionFactory _factory;

    public MetricMetadataRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public IEnumerable<MetricMetadataEntity> Get(MetricMetadataQuery query)
    {
        using var conn = _factory.Create();

        var sql = "SELECT Tag, Key1, Key2, Key3, Data, CreatedAt, ExpiredAfter FROM MetricMetadata";
        var where = new List<string>();
        var param = new DynamicParameters();

        where.Add("Tag = @Tag");
        param.Add("@Tag", query.Tag);

        where.Add("Key1 = @Key1");
        param.Add("@Key1", query.Key1);

        where.Add("Key2 = @Key2");
        param.Add("@Key2", query.Key2);

        where.Add("Key3 = @Key3");
        param.Add("@Key3", query.Key3);

        sql += " WHERE " + string.Join(" AND ", where);

        var result = conn.Query<MetricMetadataEntity>(new CommandDefinition(sql, param));

        return result;
    }

    public void Upsert(MetricMetadataEntity entity)
    {
        using var conn = _factory.Create();
        
        var sql = @"
INSERT INTO MetricMetadata (
  Tag,
  Key1,
  Key2,
  Key3,
  Data,
  CreatedAt,
  ExpiredAfter
)
VALUES (
  @Tag,
  @Key1,
  @Key2,
  @Key3,
  @Data,
  @CreatedAt,
  @ExpiredAfter
)
ON CONFLICT(Tag, Key1, Key2, Key3)
DO UPDATE SET
  Data = excluded.Data,
  CreatedAt = excluded.CreatedAt,
  ExpiredAfter = excluded.ExpiredAfter
;";

        var param = new
        {
            entity.Tag,
            entity.Key1,
            entity.Key2,
            entity.Key3,
            entity.Data,
            entity.CreatedAt,
            entity.ExpiredAfter
        };

        conn.Execute(new CommandDefinition(sql, param));
    }

    public void Delete(string tag, string key1, string key2, string key3)
    {
        using var conn = _factory.Create();

        var sql = @"DELETE FROM MetricMetadata WHERE Tag = @Tag AND Key1 = @Key1 AND Key2 = @Key2 AND Key3 = @Key3;";
        var param = new
        {
            Tag = tag,
            Key1 = key1,
            Key2 = key2,
            Key3 = key3,
        };

        conn.Execute(new CommandDefinition(sql, param));
    }
}