using Newtonsoft.Json.Linq;

namespace StatPresentation.Dtos;

public sealed record BaseQueryDto
{
    public long UserId { get; init; }
    public Guid? UserGroupId { get; init; }
    
    public JObject? Payload { get; init; }
}