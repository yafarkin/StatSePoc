using DataService.Interfaces;
using DataService.Interfaces.Api;

namespace DataService.Impl.Api;

internal sealed class DataService : IDataService
{
    public ISampleDataService SampleApi { get; }
    
    public DataService(ISampleDataService sampleApi)
    {
        SampleApi = sampleApi;
    }

    public string GetServerTime()
    {
        var result = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        return result;
    }
}