namespace DataService.Entities;

internal sealed record MetricValueEntity
{
    public string Id { get; private set; } = null!;
    
    public int UserId { get; init; }
    public string? UserGroupId { get; init; }

    public string Tag { get; init; } = null!;
    public string MetricName { get; init; } = null!;
    public double Value { get; init; }
    public string CreatedAt { get; init; } = null!;

    public void SetFromUlid(Ulid ulid)
    {
        if (!string.IsNullOrWhiteSpace(Id) && Id != Ulid.Empty.ToString())
        {
            throw new  Exception("Cannot set id for a MetricValueEntity");
        }
        
        Id = ulid.ToString();
    }

    public void GenerateId()
    {
        SetFromUlid(Ulid.NewUlid());
    }
}