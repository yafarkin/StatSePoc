using BenchmarkDotNet.Attributes;
using Jint;

namespace Benchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[ExceptionDiagnoser]
public class JitEngineBenchmark
{
    private int _counter1;
    private int _counter2;
    
    [GlobalSetup]
    public void Setup()
    {
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Console.WriteLine($"*** Final: Counter1={_counter1}, Counter2={_counter2}");
    }

    private long SomeMethodFromJs()
    {
        Interlocked.Increment(ref _counter1);
        return DateTime.UtcNow.Ticks;
    }

    [Benchmark]
    public object? RunScript()
    {
        Interlocked.Increment(ref _counter2);

        var engine = new Engine(cfg => cfg
            .LimitRecursion(10)
            .MaxStatements(1000)
            .TimeoutInterval(TimeSpan.FromSeconds(5))
        );
        
        engine.Execute("function handle() {var x = 1 + 2 + 3 + someMethod(); return Date.now();}");

        engine.SetValue("someMethod", SomeMethodFromJs);

        var result = engine.Invoke("handle").ToObject();
        return result;
    }
}