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
    private readonly IMetricMetadataRepository _metadataRepository;

    public MetricDataService(
        IMetricValueServiceMetrics metrics,
        IMetricValueRepository valueRepository,
        IMetricEventRepository eventRepository,
        IMetricMetadataRepository metadataRepository)
    {
        _metrics = metrics;
        _valueRepository = valueRepository;
        _eventRepository = eventRepository;
        _metadataRepository = metadataRepository;
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

    public MetricMetadataDto? GetMetricMetadata(string tag, string key1, string? key2, string? key3)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var query = new MetricMetadataQuery
            {
                Tag = tag,
                Key1 = key1,
                Key2 = key2 ?? string.Empty,
                Key3 = key3 ?? string.Empty ,
            };

            var metrics = _metadataRepository.Get(query);

            var metricsDto = metrics
                .Select(MetricMetadataMapper.ToDto)
                .FirstOrDefault();

            _metrics.IncExecution("GetMetricMetadata", "success");

            return metricsDto;
        }
        catch
        {
            _metrics.IncExecution("GetMetricMetadata", "error");
            throw;
        }
        finally
        {
            _metrics.ObserverDuration("GetMetricMetadata", sw.Elapsed);
        }
    }

    public void UpsertMetricMetadata(string tag, string key1, string? key2, string? key3, string? data, DateTime? expiredAfter)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var dto = GetMetricMetadata(tag, key1, key2, key3) ?? new MetricMetadataDto
            {
                Tag = tag,
                Key1 = key1,
                Key2 = key2 ?? string.Empty,
                Key3 = key3 ?? string.Empty,
            };
            
            dto = dto with
            {
                Data = data,
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiredAfter = expiredAfter
            };

            var entity = MetricMetadataMapper.ToEntity(dto);
            
            _metadataRepository.Upsert(entity);

            _metrics.IncExecution("UpsertMetricMetadata", "success");
        }
        catch
        {
            _metrics.IncExecution("UpsertMetricMetadata", "error");
            throw;
        }
        finally
        {
            _metrics.ObserverDuration("UpsertMetricMetadata", sw.Elapsed);
        }
    }

    public void DeleteMetricMetadata(string tag, string key1, string? key2, string? key3)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            _metadataRepository.Delete(tag, key1, key2 ?? string.Empty, key3 ?? string.Empty);

            _metrics.IncExecution("DeleteMetricMetadata", "success");
        }
        catch
        {
            _metrics.IncExecution("DeleteMetricMetadata", "error");
            throw;
        }
        finally
        {
            _metrics.ObserverDuration("DeleteMetricMetadata", sw.Elapsed);
        }
    }
}