namespace ScriptProviderService.Dtos;

internal sealed record ScriptKey
{
    public string Path { get; init; } = null!;
    public long Version { get; init; }
}