using MediatR;
using Newtonsoft.Json;
using ScriptService.Interfaces;

namespace ScriptService.Handlers.RunScript;

internal sealed class RunScriptQueryHandler : IRequestHandler<RunScriptQuery, object?>
{
    private readonly IScriptExecutor _executor;

    public RunScriptQueryHandler(IScriptExecutor executor)
    {
        _executor = executor;
    }

    public Task<object?> Handle(RunScriptQuery request, CancellationToken cancellationToken)
    {
        var json = JsonConvert.SerializeObject(request);
        
        var result = _executor.Execute(
            request.Tag,
            request.ScriptName,
            json);

        return Task.FromResult(result);
    }
}