using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using Skrypt.ANTLR;
using System.IO;

namespace Skrypt {
    class Program {
        static void Main(string[] args){
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Code\\test.skt");

            var engine = new Engine();

            engine.SetValue("print", (e, s, i) => {
                if (i[0].Members.ContainsKey("string")) {
                    Console.WriteLine((i[0].Members["string"].Value as FunctionInstance).Function.Run(engine, null, Arguments.Empty));
                } else {
                    Console.WriteLine(i[0]);
                }

                return null;
            });

            engine.SetValue("benchmark", (e, s, i) => {
                var function = i.GetAs<FunctionInstance>(0);
                var amount = i.GetAs<NumberInstance>(1).Value;

                var sw = System.Diagnostics.Stopwatch.StartNew();

                for (int x = 0; x < amount; x++) {
                    function.Function.Run(engine, null, Arguments.Empty);
                }

                sw.Stop();

                Console.WriteLine($"Executed function {amount} times in {sw.Elapsed.TotalMilliseconds}ms");
                Console.WriteLine($"Equals {1 / sw.Elapsed.TotalSeconds * amount} times per second");
                Console.WriteLine($"Average {(sw.Elapsed.TotalMilliseconds / amount).ToString(".####################")}ms");

                return null;
            });

            engine.DoFile(path).CreateGlobals();
            
            while (true) {
                string line = Console.ReadLine();

                if (line == "exit") break;

                try {
                    Console.WriteLine(engine.Run(line).CreateGlobals().CompletionValue);
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
