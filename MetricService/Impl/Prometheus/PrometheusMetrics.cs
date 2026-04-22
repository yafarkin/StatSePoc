using MetricService.Interfaces;
using Prometheus;

namespace MetricService.Impl.Prometheus;

internal sealed class PrometheusMetrics : IScriptMetrics, IMetricValueServiceMetrics
{
    private Counter _scriptExecutionCounter;
    private Histogram _scriptDurationHistogram;

    private Counter _metricValueExecutionCounter;
    private Histogram _metricValueDurationHistogram;

    public PrometheusMetrics()
    {
        CreateScriptMetrics();
        CreateMetricValueMetrics();
    }

    private void CreateMetricValueMetrics()
    {
        _metricValueExecutionCounter = Metrics.CreateCounter(
            "metric_value_executions_total",
            "Total number of metric value api executions",
            new CounterConfiguration
            {
                LabelNames = ["method", "status"]
            });

        _metricValueDurationHistogram = Metrics.CreateHistogram(
            "metric_value_duration_seconds",
            "Metric value api execution duration",
            new HistogramConfiguration
            {
                LabelNames = ["method"],

                Buckets = Histogram.ExponentialBuckets(
                    start: 0.001,   // 1 ms
                    factor: 2,
                    count: 15
                )
            });
    }

    private void CreateScriptMetrics()
    {
        _scriptExecutionCounter = Metrics.CreateCounter(
            "script_executions_total",
            "Total number of script executions",
            new CounterConfiguration
            {
                LabelNames = ["tag", "script", "status"]
            });

        _scriptDurationHistogram = Metrics.CreateHistogram(
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
        _scriptExecutionCounter
            .WithLabels(tag, script, status)
            .Inc();
    }

    public void ObserverDuration(string tag, string script, TimeSpan duration)
    {
        _scriptDurationHistogram
            .WithLabels(tag, script)
            .Observe(duration.TotalSeconds);
    }

    public void IncExecution(string method, string status)
    {
        _metricValueExecutionCounter
            .WithLabels(method, status)
            .Inc();
    }

    public void ObserverDuration(string method, TimeSpan duration)
    {
        _metricValueDurationHistogram
            .WithLabels(method)
            .Observe(duration.TotalSeconds);
    }
}