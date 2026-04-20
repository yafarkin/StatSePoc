namespace DataService.Interfaces;

public interface IDataService
{
    ISampleDataService SampleApi { get; }
    
    string GetServerTime();
}