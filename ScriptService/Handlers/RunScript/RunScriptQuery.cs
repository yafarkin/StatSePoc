using MediatR;
using Newtonsoft.Json.Linq;

namespace ScriptService.Handlers.RunScript;

public sealed record RunScriptQuery : IRequest<object?>
{
    public string Tag { get; init; } = null!;
    public string ScriptName { get; init; } = null!;
    
    public long UserId { get; init; }
    public Guid? UserGroupId { get; init; }
    
    public string? MetricName { get; init; }
    
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }

    public JObject? Payload { get; init; }
}