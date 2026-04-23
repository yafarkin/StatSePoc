namespace DataService.Dtos.Queries;

internal sealed record MetricValueQuery
{
    public string? Id { get; init; }
    public string? Tag { get; init; }
    public long? UserId { get; init; }
    public Guid? UserGroupId { get; init; }
    public string? MetricName { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}