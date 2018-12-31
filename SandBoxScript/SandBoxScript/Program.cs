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

            Console.WriteLine(engine.Run(input));

            Console.ReadKey();
        }
    }
}
