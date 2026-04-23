using DataService.Dtos.Metrics;
using DataService.Entities;

namespace DataService.Mappers;

internal static class MetricMetadataMapper
{
    public static MetricMetadataDto ToDto(MetricMetadataEntity entity)
    {
        var result = new MetricMetadataDto
        {
            Tag = entity.Tag,
            Key1 = entity.Key1,
            Key2 = entity.Key2,
            Key3 = entity.Key3,
            CreatedAt = DateTimeOffset.Parse(entity.CreatedAt),
            ExpiredAfter = string.IsNullOrWhiteSpace(entity.ExpiredAfter) ? null : DateTimeOffset.Parse(entity.ExpiredAfter),
            Data =  entity.Data,
        };

        return result;
    }

    public static MetricMetadataEntity ToEntity(MetricMetadataDto dto)
    {
        var result = new MetricMetadataEntity
        {
            Tag = dto.Tag,
            Key1 = dto.Key1,
            Key2 = dto.Key2,
            Key3 = dto.Key3,
            CreatedAt = dto.CreatedAt.ToString("O"),
            ExpiredAfter = dto.ExpiredAfter?.ToString("O"),
            Data = dto.Data,
        };

        return result;
    }
}