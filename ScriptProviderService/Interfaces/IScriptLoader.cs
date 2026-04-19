namespace ScriptProviderService.Interfaces;

public interface IScriptLoader
{
    string Load(string tag, string scriptName);
}