using ScriptProviderService.Dtos;

namespace ScriptProviderService.Interfaces;

internal interface IScriptCatalog
{
    ScriptKey? Exist(string path);
}