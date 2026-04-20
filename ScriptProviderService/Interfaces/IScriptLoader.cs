using ScriptProviderService.Dtos;

namespace ScriptProviderService.Interfaces;

public interface IScriptLoader
{
    Script Load(string tag, string scriptName);
}