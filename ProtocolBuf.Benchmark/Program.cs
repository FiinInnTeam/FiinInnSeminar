using BenchmarkDotNet.Running;
using System;

namespace ProtocolBuf.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SerializationBenchmark>();
        }
    }
}
