namespace DataService.Dtos.Queries;

public sealed record MetricValueQuery
{
    public Ulid Id { get; init; } = Ulid.Empty;
    public string? Tag { get; init; }
    public int? UserId { get; init; }
    public Guid? UserGroupId { get; init; }
    public string? MetricName { get; init; }
    public DateTimeOffset? StartDate { get; init; }
    public DateTimeOffset? EndDate { get; init; }
}