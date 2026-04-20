using System.Collections.Concurrent;
using DataService.Interfaces;
using Jint;
using Jint.Runtime;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScriptProviderService.Interfaces;
using ScriptService.Interfaces;

namespace ScriptService.Impl;

internal sealed class ScriptExecutor : IScriptExecutor
{
    private readonly IScriptLoader _scriptLoader;
    private readonly IDataService _dataService;
    private readonly ILogger<ScriptExecutor> _logger;
    
    private string _tag;
    
    private readonly ConcurrentDictionary<string, object?> _moduleCache = new();    
    
    public ScriptExecutor(
        ILogger<ScriptExecutor> logger,
        IDataService dataService,
        IScriptLoader scriptLoader)
    {
        _logger = logger;
        _dataService = dataService;
        _scriptLoader = scriptLoader;
    }

    public object? Execute(string tag, string scriptName, string? json)
    {
        _tag = tag;
        
        var data = string.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject(json);
        
        var script = _scriptLoader.Load(tag, scriptName);
        
        var engine = CreateEngine(scriptName);
        
        engine.SetValue("data", data);
        engine.Execute(script.Data.Code);

        var result = engine.Invoke("handle", data).ToObject();
        
        return result;
    }

    private Engine CreateEngine(string scriptName)
    {
        var engine = new Engine(cfg => cfg
            .LimitRecursion(10)
            .MaxStatements(1000)
            //.TimeoutInterval(TimeSpan.FromSeconds(5))
            .TimeoutInterval(TimeSpan.FromMinutes(5))
        );
        
        var exports = engine.Evaluate("({})");
        engine.SetValue("exports", exports);
        
        engine.SetValue("api", _dataService);
        
        engine.SetValue("log", new Action<object>(msg =>
        {
            _logger.LogInformation("[SCRIPT][{tag}/{name}] {Message}", _tag, scriptName, msg);
        }));

        engine.SetValue("require",
            new Func<string, object?>(requireScriptName =>
                _moduleCache.GetOrAdd($"{_tag}/{requireScriptName}", name => LoadRequiredScript(_tag, requireScriptName))));

        engine.SetValue("require_base",
            new Func<string, object?>(requireScriptName =>
                _moduleCache.GetOrAdd(requireScriptName, name => LoadRequiredScript(string.Empty, requireScriptName))));

        return engine;
    }

    private object LoadRequiredScript(string tag, string scriptName)
    {
        var script = _scriptLoader.Load(tag, scriptName);

        var childEngine = CreateEngine(scriptName);
        
        // var exports = new Jint.Native.Object.ObjectInstance(childEngine);
        // childEngine.SetValue("exports", exports);        

        childEngine.Execute(script.Data.Code);

        return childEngine.GetValue("exports");
    }
}