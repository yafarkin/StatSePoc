using ScriptProviderService.Dtos;

namespace ScriptProviderService.Interfaces;

internal interface IScriptCatalog
{
    IEnumerable<ScriptKey> List();
    ScriptKey? Exist(string path);
}