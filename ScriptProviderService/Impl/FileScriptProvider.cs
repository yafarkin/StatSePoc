using ScriptProviderService.Dtos;
using ScriptProviderService.Interfaces;

namespace ScriptProviderService.Impl;

internal sealed class FileScriptProvider : IScriptProvider, IScriptCatalog
{
    private readonly string _root = Path.Combine(
        AppContext.BaseDirectory,
        "UserScripts");

    private string GetFullPath(string path)
    {
        var full = Path.GetFullPath(Path.Combine(_root, path));

        return !full.StartsWith(_root) ? throw new FileNotFoundException($"Invalid script path: {path}") : full;
    }

    public IEnumerable<ScriptKey> List()
    {
        var files = Directory.GetFiles(_root, "*.js", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            yield return Exist(file)!;
        }
    }

    public ScriptKey? Exist(string path)
    {
        var fileName = GetFullPath(path);

        var result = File.Exists(fileName);
        if (!result)
        {
            return null;
        }
        
        var version = File.GetLastWriteTimeUtc(fileName).Ticks;
        return new ScriptKey
        {
            Path = fileName,
            Version = version,
        };
    }

    public ScriptData Load(ScriptKey scriptKey)
    {
        var fileName = GetFullPath(scriptKey.Path);
        
        var code = File.ReadAllText(fileName);
        
        return new ScriptData
        {
            Code = code,
        };
    }
}