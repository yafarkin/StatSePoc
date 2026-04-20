namespace ScriptProviderService.Dtos;

public sealed record Script
{
    public ScriptKey Key { get; init; } = null!;
    public ScriptData Data { get; init; } = null!;
}