namespace DataService.Entities;

internal sealed record MetricMetadataEntity
{
    public string Tag { get; init; } = null!;
    public string Key1 { get; init; } = null!;
    public string Key2 { get; init; } = string.Empty;
    public string Key3 { get; init; } = string.Empty;
    public string CreatedAt { get; init; } = null!;
    public string? ExpiredAfter { get; init; }
    public string? Data { get; init; }
}