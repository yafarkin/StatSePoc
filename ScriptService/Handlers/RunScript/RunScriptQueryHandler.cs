using MediatR;
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
        var result = _executor.Execute(
            request.Tag,
            request.ScriptName,
            request.Json);

        return Task.FromResult(result);
    }
}