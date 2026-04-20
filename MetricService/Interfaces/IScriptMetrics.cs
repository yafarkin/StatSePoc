namespace MetricService.Interfaces;

public interface IScriptMetrics
{
    void IncExecution(string tag, string script, string status);
    void ObserverDuration(string tag, string script, TimeSpan duration);
}