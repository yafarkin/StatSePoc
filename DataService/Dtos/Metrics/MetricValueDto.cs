namespace DataService.Dtos.Metrics;

public sealed record MetricValueDto
{
    public Ulid Id { get; init; }
    
    public long UserId { get; init; }
    public Guid? UserGroupId { get; init; }

    public string Tag { get; init; } = null!;
    public string MetricName { get; init; } = null!;
    public double Value { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}