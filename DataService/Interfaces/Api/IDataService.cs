namespace DataService.Interfaces.Api;

public interface IDataService
{
    ISampleDataService SampleApi { get; }
    IMetricDataService MetricApi { get; }
    
    string GetServerTime();
}