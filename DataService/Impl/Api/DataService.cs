using DataService.Interfaces.Api;

namespace DataService.Impl.Api;

internal sealed class DataService : IDataService
{
    public ISampleDataService SampleApi { get; }
    public IMetricDataService MetricApi { get; }

    public DataService(ISampleDataService sampleApi, IMetricDataService metricApi)
    {
        SampleApi = sampleApi;
        MetricApi = metricApi;
    }

    public string GetServerTime()
    {
        var result = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        return result;
    }
}