namespace DataService.Dtos.SampleData;

public sealed record SampleDataRequest
{
    public string? Text { get; init; }
    public int? Number { get; init; }
    
    public int[]? Arr { get; init; }
    
    public SampleDataRequestInner? Inner { get; init; }
}