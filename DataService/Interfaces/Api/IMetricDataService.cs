using DataService.Dtos.Metrics;

namespace DataService.Interfaces.Api;

public interface IMetricDataService
{
    MetricValueDto[] GetMetricValues(string? tag, int? userId, string? userGroupId, string? metricName, string? startDate, string? endDate);
    MetricEventDto[] GetMetricEvents(string? id, string? tag, int? userId, string? userGroupId, string? metricName, string? startDate, string? endDate);
    
    MetricMetadataDto? GetMetricMetadata(string tag, string key1, string? key2, string? key3);
    void UpsertMetricMetadata(string tag, string key1, string? key2, string? key3, string? data, DateTime? expiredAfter);
    void DeleteMetricMetadata(string tag, string key1, string? key2, string? key3);
}