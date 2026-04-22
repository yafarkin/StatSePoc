using Dapper;
using DataService.Dtos.Queries;
using DataService.Entities;
using DataService.Interfaces;
using DataService.Interfaces.Repositories;

namespace DataService.Impl.Db.Repositories;

internal sealed class MetricValueRepository : IMetricValueRepository
{
    private readonly IDbConnectionFactory _factory;

    public MetricValueRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public IEnumerable<MetricValueEntity> Get(MetricValueQuery query)
    {
        using var conn = _factory.Create();

        var sql = "SELECT Tag, UserId, UserGroupId, MetricName, Value, CreatedAt FROM MetricValue";
        var where = new List<string>();
        var param = new DynamicParameters();

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

        var result = conn.Query<MetricValueEntity>(new CommandDefinition(sql, param));

        return result;
    }

    public async Task UpsertAsync(
        MetricValueEntity entity,
        CancellationToken cancellationToken = default)
    {
        using var conn = _factory.Create();

        var sql = @"
INSERT INTO MetricValue (
  Tag,
  UserId,
  UserGroupId,
  MetricName,
  Value,
  CreatedAt
)
VALUES (
  @Tag,
  @UserId,
  @UserGroupId,
  @MetricName,
  @Value,
  @CreatedAt
)
ON CONFLICT(Tag, UserId, MetricName, CreatedAt)
DO UPDATE SET
  Value = excluded.Value
";

        var param = new
        {
            entity.Tag,
            entity.UserId,
            entity.UserGroupId,
            entity.MetricName,
            entity.Value,
            entity.CreatedAt
        };

        await conn.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: cancellationToken));
    }
}