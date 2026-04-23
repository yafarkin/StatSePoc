using Dapper;
using DataService.Dtos.Queries;
using DataService.Entities;
using DataService.Interfaces;
using DataService.Interfaces.Repositories;

namespace DataService.Impl.Db.Repositories;

internal sealed class MetricEventRepository : IMetricEventRepository
{
    private readonly IDbConnectionFactory _factory;

    public MetricEventRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public IEnumerable<MetricEventEntity> Get(MetricValueQuery query)
    {
        using var conn = _factory.Create();

        var sql = "SELECT Id, Tag, UserId, UserGroupId, MetricName, Value, CreatedAt FROM MetricEvent";
        var where = new List<string>();
        var param = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(query.Id))
        {
            where.Add("Id = @Id");
            param.Add("@Id", query.Id);
        }

        if (!string.IsNullOrWhiteSpace(query.Tag))
        {
            where.Add("Tag = @Tag");
            param.Add("@Tag", query.Tag);
        }

        if (query.UserId is not null)
        {
            where.Add("UserId = @UserId");
            param.Add("@UserId", query.UserId.Value);
        }

        if (query.UserGroupId is not null)
        {
            where.Add("UserGroupId = @UserGroupId");
            param.Add("@UserGroupId", query.UserGroupId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.MetricName))
        {
            where.Add("MetricName = @MetricName");
            param.Add("@MetricName", query.MetricName);
        }

        if (query.StartDate is not null)
        {
            where.Add("CreatedAt >= @StartDate");
            param.Add("@StartDate", query.StartDate.Value.ToString("O"));
        }

        if (query.EndDate is not null)
        {
            where.Add("CreatedAt <= @EndDate");
            param.Add("@EndDate", query.EndDate.Value.ToString("O"));
        }

        if (where.Count > 0)
        {
            sql += " WHERE " + string.Join(" AND ", where);
        }

        var result = conn.Query<MetricEventEntity>(new CommandDefinition(sql, param));

        return result;
    }

    public async Task<Ulid> CreateAsync(MetricEventEntity entity, CancellationToken cancellationToken)
    {
        using var conn = _factory.Create();
        
        var id = Ulid.NewUlid();

        var sql = @"
INSERT INTO MetricEvent (
  Id,
  Tag,
  UserId,
  UserGroupId,
  MetricName,
  Value,
  CreatedAt
)
VALUES (
  @Id,
  @Tag,
  @UserId,
  @UserGroupId,
  @MetricName,
  @Value,
  @CreatedAt
);";

        var param = new
        {
            id,
            entity.Tag,
            entity.UserId,
            entity.UserGroupId,
            entity.MetricName,
            entity.Value,
            entity.CreatedAt
        };

        await conn.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: cancellationToken));

        return id;
    }
}