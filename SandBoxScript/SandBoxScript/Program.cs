using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using SandBoxScript.ANTLR;
using System.IO;

namespace SandBoxScript {
    class Program {
        static void Main(string[] args){
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Code\\test.skt");

            string input = File.ReadAllText(path);

            var engine = new Engine();

            engine.SetValue("print", (e, s, i) => {
                Console.WriteLine(i[0]);

                return null;
            });

            engine.Run(input);

            //var update = engine.GetValue("Update") as FunctionInstance;

            //var sw = System.Diagnostics.Stopwatch.StartNew();
            //var amnt = 10000;

            //for (int i = 0; i < amnt; i++) {
            //    update.Function.Run(engine,null,null);
            //}

            //sw.Stop();

            //Console.WriteLine($"Executed function {amnt} times in {sw.Elapsed.TotalMilliseconds}ms");
            //Console.WriteLine($"Equals {1 / sw.Elapsed.TotalSeconds * amnt} times per second");
            //Console.WriteLine($"Average {(sw.Elapsed.TotalSeconds / amnt).ToString(".####################")}ms");

            Console.ReadKey();
        }
    }
}
