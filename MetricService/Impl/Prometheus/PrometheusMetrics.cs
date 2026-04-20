using MetricService.Interfaces;
using Prometheus;

namespace MetricService.Impl.Prometheus;

internal sealed class PrometheusMetrics : IScriptMetrics
{
    private readonly Counter _executionCounter;
    private readonly Histogram _durationHistogram;

    public PrometheusMetrics()
    {
        _executionCounter = Metrics.CreateCounter(
            "script_executions_total",
            "Total number of script executions",
            new CounterConfiguration
            {
                LabelNames = ["tag", "script", "status"]
            });

        _durationHistogram = Metrics.CreateHistogram(
            "script_execution_duration_seconds",
            "Script execution duration",
            new HistogramConfiguration
            {
                LabelNames = ["tag", "script"],

                Buckets = Histogram.ExponentialBuckets(
                    start: 0.001,   // 1 ms
                    factor: 2,
                    count: 15
                )
            });        
    }
    
    public void IncExecution(string tag, string script, string status)
    {
        _executionCounter
            .WithLabels(tag, script, status)
            .Inc();
    }

    public void ObserverDuration(string tag, string script, TimeSpan duration)
    {
        _durationHistogram
            .WithLabels(tag, script)
            .Observe(duration.TotalSeconds);
    }
}