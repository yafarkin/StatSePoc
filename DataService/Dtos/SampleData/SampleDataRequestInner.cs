namespace DataService.Dtos.SampleData;

public sealed record SampleDataRequestInner
{
    public string? Guid { get; init; }
    
    internal Guid? GuidValue => string.IsNullOrWhiteSpace(Guid) ? null : new Guid(Guid);
}