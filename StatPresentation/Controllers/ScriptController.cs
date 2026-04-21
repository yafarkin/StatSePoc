using System.Text.Json;
using Jint.Runtime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScriptService.Handlers.RunScript;
using ScriptService.Interfaces;
using StatPresentation.Dtos;

namespace StatPresentation.Controllers;

[ApiController]
[Route("run")]
public class ScriptController : ControllerBase
{
    private readonly IScriptExecutor _scriptExecutor;
    private readonly bool _isDevelopment;
    
    private readonly IMediator _mediator;

    public ScriptController(
        IScriptExecutor scriptExecutor,
        IWebHostEnvironment hostingEnvironment,
        IMediator mediator)
    {
        _scriptExecutor = scriptExecutor;
        _mediator = mediator;
        _isDevelopment = hostingEnvironment.IsDevelopment();
    }

    [HttpPost("{tag}/{userId}/{scriptName}")]
    public async Task<IActionResult> Run(string tag, long userId, string scriptName, [FromBody] BaseQueryDto jsQuery, CancellationToken cancellationToken)
    {
        try
        {
            var query = new RunScriptQuery
            {
                Tag = tag,
                ScriptName = scriptName,
                UserId = userId,
                UserGroupId = jsQuery.UserGroupId,
                Payload = jsQuery.Payload,
            };

            var result = await _mediator.Send(query, cancellationToken);
                
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