using DataService.Interfaces;

namespace DataService.Impl;

internal sealed class DataService : IDataService
{
    public string GetVersion()
    {
        var result = typeof(DataService).Assembly.GetName().Version.ToString();
        return result;
    }

    public string GetServerTime()
    {
        var result = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        return result;
    }
}