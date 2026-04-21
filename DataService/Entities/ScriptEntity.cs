namespace DataService.Entities;

public sealed record ScriptEntity
{
    public long Id { get; set; }
    public string Tag { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
}