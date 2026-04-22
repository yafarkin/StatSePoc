namespace MetricService.Interfaces;

public interface IMetricValueServiceMetrics
{
    void IncExecution(string method, string status);
    void ObserverDuration(string method, TimeSpan duration);
}