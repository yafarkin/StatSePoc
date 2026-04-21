using DataService.Dtos.SampleData;

namespace DataService.Interfaces.Api;

public interface ISampleDataService
{
    SampleDataResponse CallSample(SampleDataRequest request);
}