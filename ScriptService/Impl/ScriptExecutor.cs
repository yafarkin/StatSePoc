using System.Collections.Concurrent;
using System.Diagnostics;
using DataService.Interfaces;
using DataService.Interfaces.Api;
using Jint;
using Jint.Runtime;
using MetricService.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScriptProviderService.Interfaces;
using ScriptService.Interfaces;

namespace ScriptService.Impl;

internal sealed class ScriptExecutor : IScriptExecutor
{
    private readonly IScriptMetrics _scriptMetrics;
    private readonly IScriptLoader _scriptLoader;
    private readonly IDataService _dataService;
    private readonly ILogger<ScriptExecutor> _logger;

    private string Tag { get; set; } = null!;
    
    private readonly ConcurrentDictionary<string, object?> _moduleCache = new();    
    
    public ScriptExecutor(
        ILogger<ScriptExecutor> logger,
        IDataService dataService,
        IScriptLoader scriptLoader,
        IScriptMetrics scriptMetrics)
    {
        _logger = logger;
        _dataService = dataService;
        _scriptLoader = scriptLoader;
        _scriptMetrics = scriptMetrics;
    }

    public object? Execute(string tag, string scriptName, string? json)
    {
        Tag = tag;
        
        var sw = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation("Executing script {Tag}/{Script}", tag, scriptName);

            var data = string.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject(json);

            var script = _scriptLoader.Load(tag, scriptName);

            var engine = CreateEngine(scriptName);
            engine.Execute(script.Data.Code);
            var result = engine.Invoke("handle", data).ToObject();

            _logger.LogInformation("Script {Tag}/{Script} executed successfully, time = {time} ms", tag, scriptName,
                sw.ElapsedMilliseconds);

            _scriptMetrics.IncExecution(tag, scriptName, "success");

            return result;
        }
        catch (JavaScriptException e)
        {
            _logger.LogError(e, "Script {Tag}/{Script} failed, time = {time} ms", tag, scriptName, sw.ElapsedMilliseconds);
            _scriptMetrics.IncExecution(tag, scriptName, "error");
            throw;
        }
        catch (TimeoutException e)
        {
            _logger.LogError(e, "Script {Tag}/{Script} timeouted, time = {time} ms", tag, scriptName, sw.ElapsedMilliseconds);
            _scriptMetrics.IncExecution(tag, scriptName, "timeout");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error in script {Tag}/{Script}, time = {time} ms", tag, scriptName, sw.ElapsedMilliseconds);
            _scriptMetrics.IncExecution(tag, scriptName, "exception");
            throw;
        }
        finally
        {
            sw.Stop();
            _scriptMetrics.ObserverDuration(tag, scriptName, sw.Elapsed);
        }
    }

    private Engine CreateEngine(string scriptName)
    {
        var engine = new Engine(cfg => cfg
            .LimitRecursion(10)
            .MaxStatements(1000)
            .TimeoutInterval(TimeSpan.FromSeconds(5))
            //.TimeoutInterval(TimeSpan.FromMinutes(5))
        );
        
        var exports = engine.Evaluate("({})");
        engine.SetValue("exports", exports);
        
        engine.SetValue("api", _dataService);
        
        engine.SetValue("log", new Action<object>(msg =>
        {
            _logger.LogInformation("[SCRIPT][{tag}/{name}] {Message}", Tag, scriptName, msg);
        }));

        engine.SetValue("require",
            new Func<string, object?>(requireScriptName =>
                _moduleCache.GetOrAdd($"{Tag}/{requireScriptName}", name => LoadRequiredScript(Tag, requireScriptName))));

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