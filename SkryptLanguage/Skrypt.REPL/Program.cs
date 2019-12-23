using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Skrypt;

namespace Skrypt.REPL {
    internal static class Program {

        private static readonly SkryptEngine _engine;
        private static string _file = null;

        static Program () {
            _engine = new SkryptEngine();

            _engine.SetValue("print", new MethodDelegate(Print));
            _engine.SetValue("input", new MethodDelegate(Input));
            _engine.SetValue("benchmark", new MethodDelegate(Benchmark));

            _engine.SetValue("error", (e, s, i) => {
                throw new FatalErrorException(i.GetAs<StringInstance>(0));
            });
        }

        static void Main(string[] args) {
            if (args.Any())
                _file = Path.Combine(Directory.GetCurrentDirectory(), args[0]);

            if (!string.IsNullOrEmpty(_file)) RunFile(_file);

            while (true) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(">");

                string line = Console.ReadLine();

                if (line == "exit") return;

                if (line.StartsWith("run ")) {
                    _file = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        line.Substring(4)
                        );

                    if (!string.IsNullOrEmpty(_file)) RunFile(_file);

                    continue;
                }

                try {
                    _engine.Execute(line);

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.WriteLine(_engine.CompletionValue?.ToString() ?? "null");
                }
                catch (SkryptException e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    _engine.ErrorHandler.ReportError(e);
                }
                catch (Exception e) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(e);
                }
            }
        }

        private static void RunFile (string file) {        
            try {
                _engine.DoFile(file);
            }
            catch (SkryptException e) {
                Console.ForegroundColor = ConsoleColor.Red;
                _engine.ErrorHandler.ReportError(e);
            }
            catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e);
            }
        }

        private static SkryptObject Print (SkryptEngine engine, SkryptObject self, Arguments arguments) {
            Console.ForegroundColor = ConsoleColor.White;

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

        private static SkryptObject Input(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            if (arguments.Length == 1) Console.WriteLine(arguments[0]);

            string fullString = "";
            string line;
            while (!String.IsNullOrWhiteSpace(line = Console.ReadLine())) {
                fullString += line;
            }

            return engine.CreateString(fullString);
        }

        private static SkryptObject Benchmark(SkryptEngine engine, SkryptObject self, Arguments arguments) {
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
