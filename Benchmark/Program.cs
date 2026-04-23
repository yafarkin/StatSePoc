using Benchmark;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<JitEngineBenchmark>();
BenchmarkRunner.Run<FullpipeBenchmark>();

// var b = new FullpipeBenchmark();
// b.Setup();
// await b.RunScript_FullRandom();
// b.Cleanup();
