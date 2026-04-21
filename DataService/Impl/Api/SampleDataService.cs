using DataService.Dtos.SampleData;
using DataService.Interfaces;
using DataService.Interfaces.Api;

namespace DataService.Impl.Api;

internal sealed class SampleDataService : ISampleDataService
{
    // public SampleDataResponse CallSample(object jsObject)
    // {
    //     // тут будет ломаться на integer, т.к. приходит double
    //     var json = JsonConvert.SerializeObject(jsObject);
    //     var request = JsonConvert.DeserializeObject<SampleDataRequest>(json);
    //     // ....
    // }

    public SampleDataResponse CallSample(SampleDataRequest request)
    {
        // тут ломается на inner, т.к. не понимает что создавать.
        var random = new Random();
        
        var result = new SampleDataResponse
        {
            Text = $"Text: {request.Text ?? "<null>"}; Number: {request.Number}; Guid: {request.Inner?.Guid}; Arr len: {request.Arr?.Length};",
            Guid = Guid.NewGuid(),
            DateTime = DateTime.UtcNow,
            IntNumber = random.Next(),
            DoubleNumber = random.NextDouble(),
        };

        return result;
    }
}