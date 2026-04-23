using DataService.Dtos.Metrics;
using DataService.Entities;

namespace DataService.Mappers;

internal static class MetricEventMapper
{
    public static MetricEventDto ToDto(MetricEventEntity entity)
    {
        var result = new MetricEventDto
        {
            Id = Ulid.Parse(entity.Id),
            Tag = entity.Tag,
            UserId = entity.UserId,
            UserGroupId = string.IsNullOrWhiteSpace(entity.UserGroupId) ? null : Guid.Parse(entity.UserGroupId),
            MetricName = entity.MetricName,
            Value = entity.Value,
            CreatedAt = DateTimeOffset.Parse(entity.CreatedAt),
        };

        return result;
    }

    public static MetricEventEntity ToEntity(MetricEventDto dto)
    {
        var result = new MetricEventEntity
        {
            Id = dto.Id.ToString(),
            Tag = dto.Tag,
            UserId = dto.UserId,
            UserGroupId = dto.UserGroupId?.ToString(),
            MetricName = dto.MetricName,
            Value = dto.Value,
            CreatedAt = dto.CreatedAt.ToString("O"),
        };

        return result;
    }
}