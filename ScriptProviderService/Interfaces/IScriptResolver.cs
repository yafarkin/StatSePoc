using ScriptProviderService.Dtos;

namespace ScriptProviderService.Interfaces;

internal interface IScriptResolver
{
    ScriptKey Resolve(string tag, string scriptName);
}