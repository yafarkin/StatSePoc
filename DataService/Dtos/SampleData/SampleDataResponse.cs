namespace DataService.Dtos.SampleData;

public record SampleDataResponse
{
    public string Text { get; init; } = null!;
    public int IntNumber { get; init; }
    public double DoubleNumber { get; init; }
    public Guid Guid { get; init; }
    public DateTime DateTime { get; init; }
}