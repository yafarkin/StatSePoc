using Jint.Runtime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ScriptService.Handlers.RunScript;

namespace StatPresentation.Controllers;

[ApiController]
[Route("run")]
public class ScriptController : ControllerBase
{
    private readonly bool _isDevelopment;
    
    private readonly IMediator _mediator;

    public ScriptController(
        IWebHostEnvironment hostingEnvironment,
        IMediator mediator)
    {
        _mediator = mediator;
        _isDevelopment = hostingEnvironment.IsDevelopment();
    }

    [HttpPost("{tag}/{userId}/{scriptName}")]
    public async Task<IActionResult> Run(
        string tag,
        long userId,
        string scriptName,
        [FromQuery] Guid? userGroupId,
        [FromQuery] string? metricName,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromBody] JObject? payload,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new RunScriptQuery
            {
                Tag = tag,
                ScriptName = scriptName,
                UserId = userId,
                UserGroupId = userGroupId,
                MetricName = metricName,
                StartDate = startDate,
                EndDate = endDate,
                Payload = payload,
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