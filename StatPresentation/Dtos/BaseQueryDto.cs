using Newtonsoft.Json.Linq;

namespace StatPresentation.Dtos;

public sealed record BaseQueryDto
{
    public Guid? UserGroupId { get; init; }
    
    public JObject? Payload { get; init; }
}