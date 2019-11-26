using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skrypt.REPL {
    class Program {
        static void Main(string[] args) {
            var path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);

            var engine = new Engine();

            engine.SetValue("print", (e, s, i) => {

                var str = "";

                for (var j = 0; j < i.Length; j++) {
                    if (i[j] == null) {
                        str += "null";
                    }
                    else if (i[j].Members.ContainsKey("string")) {
                        str += (i[j].Members["string"].value as FunctionInstance).Function.Run(engine, i[j], Arguments.Empty).ToString();
                    }
                    else {
                        str += i[j].ToString();
                    }

                    if (j < i.Length - 1) str += ", ";
                }

                Console.WriteLine(str);

                return null;
            });

            engine.SetValue("input", (e, s, i) => {
                if (i.Length == 1) Console.WriteLine(i[0]);

                string fullString = "";
                string line;
                while (!String.IsNullOrWhiteSpace(line = Console.ReadLine())) {
                    fullString += line;
                }

                return engine.CreateString(fullString);
            });

            engine.SetValue("error", (e, s, i) => {
                throw new FatalErrorException(i.GetAs<StringInstance>(0));
            });

            engine.SetValue("benchmark", (e, s, i) => {
                var function = i.GetAs<FunctionInstance>(0);
                var amount = i.GetAs<NumberInstance>(1).Value;
                var lastResult = default(BaseObject);

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
            });

            engine.SetValue("log", new Action<object>(Console.WriteLine));

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
    }
}
