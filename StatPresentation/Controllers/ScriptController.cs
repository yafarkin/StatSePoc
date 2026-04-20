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
        try
        {
            var result = _scriptExecutor.Execute(
                tag: tag,
                scriptName: scriptName,
                json: body?.GetRawText());

            return Ok(result);
        }
        catch (JavaScriptException e)
        {
            return BadRequest(_isDevelopment ? e.ToString() : e.Message);
        }
        catch (Exception e)
        {
            return Problem(_isDevelopment ? e.ToString() : e.Message);
        }
    }
}