using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Skrypt;
using Skrypt.CLR;

namespace Skrypt.Benchmarks {
    [MemoryDiagnoser]
    public class EngineBenchmarks {
        private readonly SkryptEngine _engine;

        public EngineBenchmarks() {
            _engine = new SkryptEngine();

            // Pre-set value to make sure it exists in the GetValue benchmark
            _engine.SetValue("A", 1);

            _engine.Execute(@"
fn fibonacci(num) {
    if (num <= 1) return 1

    return fibonacci(num - 1) + fibonacci(num - 2)
}
            ");
        }

        [Benchmark]
        public SkryptEngine ExecuteNumericalExpression() {
            return _engine.Execute(@"result = ((A - 2) * 4) / 2");
        }

        [Benchmark]
        public SkryptEngine ExecuteStringExpression() {
            return _engine.Execute(@"result = ""Hello "" + ""world!""");
        }

        [Benchmark]
        public SkryptEngine ExecuteFib8Function() {
            return _engine.Execute(@"result = fibonacci(8)");
        }

        [Benchmark]
        public SkryptEngine ExecuteVectorExpression() {
            return _engine.Execute(@"result = ((Vector(1,2,3) - Vector(3,2,1)) * 4).Normalized");
        }
    }

    public class Program {
        public static void Main(string[] args) {
            var summary = BenchmarkRunner.Run<EngineBenchmarks>();

            Console.ReadKey();
        }
    }
}
