namespace DataService.Dtos.Metrics;

public sealed record MetricEventDto
{
    public Ulid Id { get; init; }
    public string Tag { get; init; } = null!;
    public long UserId { get; init; }
    public string MetricName { get; init; } = null!;
    public DateTimeOffset CreatedAt { get; init; }
    public Guid? UserGroupId { get; init; }
    public double Value { get; init; }
}