using ScriptProviderService.Dtos;

namespace ScriptProviderService.Interfaces;

public interface IScriptLoader
{
    void LoadAll();
    Script Load(string tag, string scriptName);
}