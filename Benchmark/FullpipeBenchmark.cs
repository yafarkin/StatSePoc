using System.Net.Http.Json;
using System.Text;
using BenchmarkDotNet.Attributes;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace Benchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class FullpipeBenchmark
{
    private WebApplicationFactory<StatPresentation.Program> _factory;
    private HttpClient _client;
    private Faker _faker;
    private string[] _tags;
    private string[] _metricNames;
    private string[] _scriptNames;
    private long[] _userIds;
    private Guid[] _userGroupIds;
    private DateOnly _startDate;
    private DateOnly _endDate;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new WebApplicationFactory<StatPresentation.Program>()
            .WithWebHostBuilder(b =>
            {
                b.UseEnvironment("Production");
                b.ConfigureLogging(logging => logging.ClearProviders());
            });
        
        _client = _factory.CreateClient();
        
        _faker = new Faker();
        
        _tags = ["click", "view", "purchase", "login", "error"];
        _metricNames = ["metric1", "metric2", "metric3", "metric4", "metric5", "metric6"];
        
        _startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1));
        _endDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }
    
    [Benchmark]
    public async Task RunScript_FullRandom()
    {
        var tag = _faker.PickRandom(_tags);
        var userId = _faker.Random.Number(0, 1000);
        var scriptName = "card";
        var metricName = _faker.Random.Bool(0.8f) ? _faker.PickRandom(_metricNames) : null;
        var startDate = _faker.Random.Bool(0.5f) ? _startDate : (DateOnly?)null;
        var endDate = _faker.Random.Bool(0.5f) ? _endDate : (DateOnly?)null;
        
        var url = $"/run/{tag}/{userId}/{scriptName}";
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(metricName))
        {
            queryParams.Add($"metricName={Uri.EscapeDataString(metricName)}");
        }

        if (startDate.HasValue)
        {
            queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
        }

        if (endDate.HasValue)
        {
            queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");
        }

        if (queryParams.Any())
        {
            url += "?" + string.Join("&", queryParams);
        }

        var emptyContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, emptyContent);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error {response.StatusCode}: {error}");
        }        
        
        response.EnsureSuccessStatusCode();
    }
}