using System.Diagnostics;
using System.Globalization;
using DataService.Dtos.Metrics;
using DataService.Dtos.Queries;
using DataService.Interfaces.Api;
using DataService.Interfaces.Repositories;
using DataService.Mappers;
using MetricService.Interfaces;

namespace DataService.Impl.Api;

internal sealed class MetricDataService : IMetricDataService
{
    private readonly IMetricValueServiceMetrics _metrics;
    private readonly IMetricValueRepository _repository;

    public MetricDataService(
        IMetricValueRepository repository,
        IMetricValueServiceMetrics metrics)
    {
        _repository = repository;
        _metrics = metrics;
    }

    public MetricValueDto[] GetMetricValues(string? id, string? tag, int? userId, string? userGroupId, string? metricName, string? startDate, string? endDate)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var query = new MetricValueQuery
            {
                Id = string.IsNullOrWhiteSpace(id) ? Ulid.Empty : Ulid.Parse(id),
                Tag = tag,
                UserId = userId,
                UserGroupId = string.IsNullOrWhiteSpace(userGroupId) ? null : Guid.Parse(userGroupId),
                MetricName = metricName,
                StartDate = string.IsNullOrWhiteSpace(startDate)
                    ? null
                    : DateTimeOffset.Parse(startDate, null, DateTimeStyles.RoundtripKind),
                EndDate = string.IsNullOrWhiteSpace(endDate)
                    ? null
                    : DateTimeOffset.Parse(endDate, null, DateTimeStyles.RoundtripKind),
            };

            var metrics = _repository.Get(query);

            var metricsDto = metrics
                .Select(MetricValueMapper.ToDto)
                .ToArray();

            _metrics.IncExecution("GetMetricValues", "success");

            return metricsDto;
        }
        catch (Exception e)
        {
            _metrics.IncExecution("GetMetricValues", "error");
            throw;
        }
        finally
        {
            _metrics.ObserverDuration("GetMetricValues", sw.Elapsed);
        }
    }
}