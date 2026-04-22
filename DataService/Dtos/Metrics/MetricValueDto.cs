namespace DataService.Dtos.Metrics;

public sealed record MetricValueDto
{
    public string Tag { get; init; } = null!;
    public long UserId { get; init; }
    public string MetricName { get; init; } = null!;
    public DateOnly CreatedAt { get; init; }

    public Guid? UserGroupId { get; init; }
    public double Value { get; init; }
}