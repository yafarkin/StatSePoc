namespace ScriptService.Interfaces;

public interface IScriptExecutor
{
    object? Execute(string tag, string scriptName, string? json);
}