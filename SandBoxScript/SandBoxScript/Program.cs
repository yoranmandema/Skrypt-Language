using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using SandBoxScript.ANTLR;

namespace SandBoxScript {
    class Program {
        static void Main(string[] args){
            string input = "Number;";
            var engine = new Engine();

            Console.WriteLine(engine.Run(input));

            Console.ReadKey();
        }
    }
}
