using MediatR;

namespace ScriptService.Handlers.RunScript;

public sealed record RunScriptQuery : IRequest<object?>
{
    public string Tag { get; init; } = null!;
    public string ScriptName { get; init; } = null!;
    public string? Json { get; init; }
}