namespace DataService.Entities;

internal sealed record MetricEventEntity
{
    public string Id { get; init; } = null!;
    public string Tag { get; init; } = null!;
    public long UserId { get; init; }
    public string MetricName { get; init; } = null!;
    public string CreatedAt { get; init; } = null!;
    public string? UserGroupId { get; init; }
    public double Value { get; init; }
}