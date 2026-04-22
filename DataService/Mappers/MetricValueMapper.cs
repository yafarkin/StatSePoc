using System.Globalization;
using DataService.Dtos.Metrics;
using DataService.Entities;

namespace DataService.Mappers;

internal static class MetricValueMapper
{
    public static MetricValueDto ToDto(MetricValueEntity entity)
    {
        var result = new MetricValueDto
        {
            Tag = entity.Tag,
            UserId = entity.UserId,
            UserGroupId = string.IsNullOrWhiteSpace(entity.UserGroupId) ? null : Guid.Parse(entity.UserGroupId),
            MetricName = entity.MetricName,
            Value = entity.Value,
            CreatedAt = DateOnly.Parse(entity.CreatedAt),
        };

        return result;
    }

    public static MetricValueEntity ToEntity(MetricValueDto dto)
    {
        var result = new MetricValueEntity
        {
            Tag = dto.Tag,
            UserId = dto.UserId,
            UserGroupId = dto.UserGroupId is null ? null : dto.UserGroupId.ToString(),
            MetricName = dto.MetricName,
            Value = dto.Value,
            CreatedAt = dto.CreatedAt.ToString("O")
        };
        
        return result;
    }
}