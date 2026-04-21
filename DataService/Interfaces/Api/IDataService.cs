namespace DataService.Interfaces.Api;

public interface IDataService
{
    ISampleDataService SampleApi { get; }
    
    string GetServerTime();
}