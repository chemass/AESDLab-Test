using BenchmarkDotNet.Running;
using Che.Coxshall;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<URLFilterBenchmark>();
    }
}