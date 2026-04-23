using System.Diagnostics;
using Benchmark;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<JitEngineBenchmark>();
//BenchmarkRunner.Run<FullpipeBenchmark>();

const int WarmupCycles = 1000;
const int TestCycles = 10_000;

var b = new FullpipeBenchmark();
b.Setup();

Console.WriteLine("Warming up...");
for (var i = 0; i < WarmupCycles; i++)
{
    await b.RunScript_FullRandom();
}

Console.WriteLine("Measuring...");
var sw = Stopwatch.StartNew();
for (var i = 0; i < TestCycles; i++)
{
    await b.RunScript_FullRandom();
}
sw.Stop();

b.Cleanup();

var avgMs = (double)sw.ElapsedMilliseconds / TestCycles;
Console.WriteLine($"Total: {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"Average: {avgMs:F3} ms/request");
Console.WriteLine($"Requests/sec: {1000 / avgMs:F0}");