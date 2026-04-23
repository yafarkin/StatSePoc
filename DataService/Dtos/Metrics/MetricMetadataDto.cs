namespace DataService.Dtos.Metrics;

public sealed record MetricMetadataDto
{
    public string Tag { get; init; } = null!;
    public string Key1 { get; init; } = null!;
    public string Key2 { get; init; } = string.Empty;
    public string Key3 { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ExpiredAfter { get; init; }
    public string? Data { get; init; }
}