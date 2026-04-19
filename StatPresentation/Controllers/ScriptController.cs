using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ScriptService.Interfaces;
using Jint.Runtime;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StatPresentation.Controllers;

[ApiController]
[Route("run")]
public class ScriptController : ControllerBase
{
    private readonly IScriptExecutor _scriptExecutor;
    private readonly ILogger<ScriptController> _logger;
    private readonly bool _isDevelopment;

    public ScriptController(
        IScriptExecutor scriptExecutor,
        ILogger<ScriptController> logger,
        IWebHostEnvironment hostingEnvironment)
    {
        _scriptExecutor = scriptExecutor;
        _logger = logger;
        _isDevelopment = hostingEnvironment.IsDevelopment();
    }

    [HttpPost("{tag}/{scriptName}")]
    public IActionResult Run(string tag, string scriptName, [FromBody] JsonElement? body)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation("Executing script {Tag}/{Script}", tag, scriptName);

            var result = _scriptExecutor.Execute(
                tag: tag,
                scriptName: scriptName,
                json: body?.GetRawText());

            _logger.LogInformation("Script {Tag}/{Script} executed successfully, time = {time} ms", tag, scriptName, sw.ElapsedMilliseconds);

            return Ok(result);
        }
        catch (JavaScriptException e)
        {
            _logger.LogError(e, "Script {Tag}/{Script} failed, time = {time} ms", tag, scriptName, sw.ElapsedMilliseconds);

            return BadRequest(_isDevelopment ? e.ToString() : e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error in script {Tag}/{Script}, time = {time} ms", tag, scriptName, sw.ElapsedMilliseconds);
            
            return Problem(_isDevelopment ? e.ToString() : e.Message);
        }
    }
}