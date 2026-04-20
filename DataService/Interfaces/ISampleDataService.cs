using DataService.Dtos.SampleData;

namespace DataService.Interfaces;

public interface ISampleDataService
{
    SampleDataResponse CallSample(SampleDataRequest request);
}