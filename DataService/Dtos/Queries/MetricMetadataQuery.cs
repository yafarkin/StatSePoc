namespace DataService.Dtos.Queries;

internal record MetricMetadataQuery
{
    public string Tag { get; init; } = null!;
    public string Key1 { get; init; } = null!;
    public string Key2 { get; init; } = string.Empty;
    public string Key3 { get; init; } = string.Empty;
}