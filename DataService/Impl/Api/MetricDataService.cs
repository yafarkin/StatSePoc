using System.Diagnostics;
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
    private readonly IMetricValueRepository _valueRepository;
    private readonly IMetricEventRepository _eventRepository;

    public MetricDataService(
        IMetricValueServiceMetrics metrics,
        IMetricValueRepository valueRepository,
        IMetricEventRepository eventRepository)
    {
        _metrics = metrics;
        _valueRepository = valueRepository;
        _eventRepository = eventRepository;
    }

    public MetricValueDto[] GetMetricValues(string? tag, int? userId, string? userGroupId, string? metricName, string? startDate, string? endDate)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var query = new MetricValueQuery
            {
                Tag = tag,
                UserId = userId,
                UserGroupId = string.IsNullOrWhiteSpace(userGroupId) ? null : Guid.Parse(userGroupId),
                MetricName = metricName,
                StartDate = string.IsNullOrWhiteSpace(startDate)
                    ? null
                    : DateOnly.Parse(startDate),
                EndDate = string.IsNullOrWhiteSpace(endDate)
                    ? null
                    : DateOnly.Parse(endDate),
            };

            var metrics = _valueRepository.Get(query);

            var metricsDto = metrics
                .Select(MetricValueMapper.ToDto)
                .ToArray();

            _metrics.IncExecution("GetMetricValues", "success");

            return metricsDto;
        }
        catch
        {
            _metrics.IncExecution("GetMetricValues", "error");
            throw;
        }
        finally
        {
            _metrics.ObserverDuration("GetMetricValues", sw.Elapsed);
        }
    }

    public MetricEventDto[] GetMetricEvents(string? id, string? tag, int? userId, string? userGroupId, string? metricName, string? startDate,
        string? endDate)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var query = new MetricValueQuery
            {
                Id = id,
                Tag = tag,
                UserId = userId,
                UserGroupId = string.IsNullOrWhiteSpace(userGroupId) ? null : Guid.Parse(userGroupId),
                MetricName = metricName,
                StartDate = string.IsNullOrWhiteSpace(startDate)
                    ? null
                    : DateOnly.Parse(startDate),
                EndDate = string.IsNullOrWhiteSpace(endDate)
                    ? null
                    : DateOnly.Parse(endDate),
            };

            var metrics = _eventRepository.Get(query);

            var metricsDto = metrics
                .Select(MetricEventMapper.ToDto)
                .ToArray();

            _metrics.IncExecution("GetMetricEvents", "success");

            return metricsDto;
        }
        catch
        {
            _metrics.IncExecution("GetMetricEvents", "error");
            throw;
        }
        finally
        {
            _metrics.ObserverDuration("GetMetricEvents", sw.Elapsed);
        }
    }
}