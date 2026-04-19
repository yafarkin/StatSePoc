using ScriptProviderService.Dtos;

namespace ScriptProviderService.Interfaces;

internal interface IScriptProvider
{
    ScriptData Load(ScriptKey scriptKey);
}