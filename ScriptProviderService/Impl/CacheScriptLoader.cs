using System.Collections.Concurrent;
using ScriptProviderService.Dtos;
using ScriptProviderService.Interfaces;

namespace ScriptProviderService.Impl;

internal sealed class CacheScriptLoader : IScriptLoader
{
    private readonly IScriptResolver _resolver;
    private readonly IScriptProvider _provider;
    private readonly ConcurrentDictionary<string, (ScriptKey, ScriptData)> _cache = new();

    public CacheScriptLoader(
        IScriptResolver resolver,
        IScriptProvider provider)
    {
        _resolver = resolver;
        _provider = provider;
    }

    public Script Load(string tag, string scriptName)
    {
        var scriptKey = _resolver.Resolve(tag, scriptName);
        var cacheKey = scriptKey.Path;

        if (_cache.TryGetValue(cacheKey, out var cached))
        {
            if (cached.Item1.Version == scriptKey.Version)
            {
                return new Script
                {
                    Key = cached.Item1,
                    Data = cached.Item2,
                };
            }
        }

        var scriptData = _provider.Load(scriptKey);
        
        _cache.AddOrUpdate(
            cacheKey,
            _ => (scriptKey, scriptData),
            (_, existing) =>
                existing.Item1.Version >= scriptKey.Version
                    ? existing
                    : (scriptKey, scriptData)
        );

        return new Script
        {
            Key = scriptKey,
            Data = scriptData,
        };
    }        
}