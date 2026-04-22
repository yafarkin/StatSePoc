using DataService.Dtos.Metrics;

namespace DataService.Interfaces.Api;

public interface IMetricDataService
{
    MetricValueDto[] GetMetricValues(string? tag, int? userId, string? userGroupId, string? metricName, string? startDate, string? endDate);
}