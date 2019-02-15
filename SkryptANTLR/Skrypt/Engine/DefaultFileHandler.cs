using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class DefaultFileHandler : IFileHandler {
        public string File { get; set; }
        public string Folder { get; set; }

        public string Read(string path) {
            string code;

            string fullPath = Path.Combine(Folder,path);

            using (StreamReader sr = new StreamReader(fullPath)) {
                code = sr.ReadToEnd();
            }

            return code;
        }

        public void Write(string destination, string content) {
            string fullPath = Path.Combine(Folder, destination);

            var directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter sr = new StreamWriter(fullPath)) {
                foreach (var c in content) { 
                    sr.Write(c);
                }
            }
        }
    }
}
