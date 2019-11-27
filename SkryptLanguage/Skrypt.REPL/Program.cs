using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Skrypt;

namespace Skrypt.REPL {
    class Program {
        static void Main(string[] args) {
            string path = null;

            if (args.Any()) {
                path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            }

            var engine = new SkryptEngine();

            engine.SetValue("print", new MethodDelegate(print));
            engine.SetValue("input", new MethodDelegate(input));
            engine.SetValue("benchmark", new MethodDelegate(benchmark));

            engine.SetValue("error", (e, s, i) => {
                throw new FatalErrorException(i.GetAs<StringInstance>(0));
            });       
            
            if (!string.IsNullOrEmpty(path))
                engine.DoFile(path).ReportErrors().CreateGlobals();

            while (true) {
                string line = Console.ReadLine();

                if (line == "exit") break;

                try {
                    Console.WriteLine(engine.Run(line).ReportErrors().CreateGlobals().CompletionValue);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }

        private static SkryptObject print (SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var str = "";

            for (var j = 0; j < arguments.Length; j++) {
                if (arguments[j] == null) {
                    str += "null";
                }
                else {
                    str += arguments[j].ToString();
                }

                if (j < arguments.Length - 1) str += ", ";
            }

            Console.WriteLine(str);

            return null;
        }

        private static SkryptObject input(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            if (arguments.Length == 1) Console.WriteLine(arguments[0]);

            string fullString = "";
            string line;
            while (!String.IsNullOrWhiteSpace(line = Console.ReadLine())) {
                fullString += line;
            }

            return engine.CreateString(fullString);
        }

        private static SkryptObject benchmark(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var function = arguments.GetAs<FunctionInstance>(0);
            var amount = arguments.GetAs<NumberInstance>(1).Value;
            var lastResult = default(SkryptObject);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            for (int x = 0; x < amount; x++) {
                lastResult = function.Function.Run(engine, null, Arguments.Empty);
            }

            sw.Stop();

            Console.WriteLine($"Executed function {amount} times in {sw.Elapsed.TotalMilliseconds}ms");
            Console.WriteLine($"Equals {1 / sw.Elapsed.TotalSeconds * amount} times per second");
            Console.WriteLine($"Average {(sw.Elapsed.TotalMilliseconds / amount).ToString(".####################")}ms");
            if (lastResult != null) Console.WriteLine($"Last result: {lastResult}");

            return null;
        }
    }
}
