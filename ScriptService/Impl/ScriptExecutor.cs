using DataService.Interfaces;
using Jint;
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
        var data = string.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject(json);
        
        var script = _scriptLoader.Load(tag, scriptName);
        
        var engine = CreateEngine(tag, scriptName);
        
        engine.SetValue("data", data);
        engine.Execute(script);

        var result = engine.Invoke("handle", data).ToObject();
        
        return result;
    }

    private Engine CreateEngine(string tag, string scriptName)
    {
        var engine = new Engine(cfg => cfg
            .LimitRecursion(10)
            .MaxStatements(1000)
            .TimeoutInterval(TimeSpan.FromSeconds(5))
        );
        
        engine.SetValue("api", _dataService);
        
        engine.SetValue("log", new Action<object>(msg =>
        {
            _logger.LogInformation("[SCRIPT][{tag}/{name}] {Message}", tag, scriptName, msg);
        }));        
        
        engine.SetValue("require", new Func<string, object?>(requireScriptName =>
        {
            var script = _scriptLoader.Load(tag, requireScriptName);

            var childEngine = CreateEngine(tag, requireScriptName);
            childEngine.Execute(script);

            return childEngine.GetValue("exports").ToObject();
        }));        

        return engine;
    }
}