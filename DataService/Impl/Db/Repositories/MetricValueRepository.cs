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

        var sql = "SELECT Id, Tag, UserId, UserIdGroup, MetricName, Value, CreatedAt FROM MetricValue";
        var where = new List<string>();
        var param = new DynamicParameters();

        if (query.Id != Ulid.Empty)
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
            where.Add("UserIdGroup = @UserIdGroup");
            param.Add("@UserIdGroup", query.UserGroupId.Value);
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

    public async Task CreateAsync(
        MetricValueEntity entity,
        CancellationToken cancellationToken = default)
    {
        using var conn = _factory.Create();

        entity.GenerateId();

        var sql = @"
INSERT INTO MetricValue (
  Id,
  Tag,
  UserId,
  UserIdGroup,
  MetricName,
  Value,
  CreatedAt
)
VALUES (
  @Id,
  @Tag,
  @UserId,
  @UserIdGroup,
  @MetricName,
  @Value,
  @CreatedAt
);";

        var param = new
        {
            entity.Id,
            entity.Tag,
            entity.UserId,
            entity.UserGroupId,
            entity.MetricName,
            entity.Value,
            entity.CreatedAt
        };

        await conn.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(
        MetricValueEntity entity,
        CancellationToken cancellationToken = default)
    {
        using var conn = _factory.Create();

        var sql = @"
UPDATE MetricValue
SET
  Tag = @Tag,
  UserId = @UserId,
  UserIdGroup = @UserIdGroup,
  MetricName = @MetricName,
  Value = @Value,
  CreatedAt = @CreatedAt
WHERE Id = @Id;";

        var param = new
        {
            entity.Id,
            entity.Tag,
            entity.UserId,
            entity.UserGroupId,
            entity.MetricName,
            entity.Value,
            entity.CreatedAt
        };

        var affected = await conn.ExecuteAsync(
            new CommandDefinition(sql, param, cancellationToken: cancellationToken));

        if (affected == 0)
        {
            throw new InvalidOperationException($"MetricValue with Id={entity.Id} not found");
        }
    }
    
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        using var conn = _factory.Create();

        await conn.ExecuteAsync(new CommandDefinition("DELETE FROM MetricValue WHERE Id = @Id", new { Id = id },
            cancellationToken: cancellationToken));
    }
}