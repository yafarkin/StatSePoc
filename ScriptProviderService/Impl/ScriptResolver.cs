using ScriptProviderService.Dtos;
using ScriptProviderService.Interfaces;

namespace ScriptProviderService.Impl;

internal sealed class ScriptResolver : IScriptResolver
{
    private readonly IScriptCatalog _provider;

    public ScriptResolver(IScriptCatalog provider)
    {
        _provider = provider;
    }

    public ScriptKey Resolve(string tag, string scriptName)
    {
        var tagPath = $"tags/{tag}/{scriptName}.js";
        var key = _provider.Exist(tagPath);
        if (key is not null)
        {
            return key;
        }

        var overridesPath = $"overrides/{scriptName}.js";
        key = _provider.Exist(overridesPath);
        if (key is not null)
        {
            return key;
        }

        var corePath = $"core/{scriptName}.js";
        key = _provider.Exist(corePath);
        if (key is not null)
        {
            return key;
        }
        
        throw new FileNotFoundException($"Script {tag}/{scriptName} could not be found.");
    }
}