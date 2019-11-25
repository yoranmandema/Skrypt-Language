using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Skrypt.Benchmarks {
    public class EngineBenchmarks {
        private readonly Engine _engine;

        public EngineBenchmarks() {
            _engine = new Engine();
            
            // Pre-set value to make sure it exists in the GetValue benchmark
            _engine.SetValue("A", 1);
        }

        [Benchmark]
        public Engine SetValue() {
            return _engine.SetValue("A", 1);
        }

        [Benchmark]
        public BaseObject GetValue() {
            return _engine.GetValue("A");
        }


    }

    public class Program {
        public static void Main(string[] args) {
            var summary = BenchmarkRunner.Run<EngineBenchmarks>();

            Console.ReadKey();
        }
    }
}
