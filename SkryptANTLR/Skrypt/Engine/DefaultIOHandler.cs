using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class DefaultIOHandler : IFileHandler {
        public string File { get; set; }
        public string Directory { get; set; }

        public string Read(string path) {
            string code;

            string fullPath = Path.Combine(Directory,path);

            using (StreamReader sr = new StreamReader(fullPath)) {
                code = sr.ReadToEnd();
            }

            return code;
        }
    }
}
