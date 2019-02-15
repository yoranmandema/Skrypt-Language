using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class DefaultFileHandler : IFileHandler {
        public Engine Engine { get; set; }
        public string File { get; set; }
        public string Folder { get; set; }

        public DefaultFileHandler (Engine e) {
            Engine = e;
        }

        public string Read(string path) {
            string str;

            string fullPath = Path.Combine(Folder,path);

            using (StreamReader sr = new StreamReader(fullPath)) {
                str = sr.ReadToEnd();
            }

            return str;
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

        public async void ReadAsync(string path, FunctionInstance callback) {
            char[] result;
            StringBuilder builder = new StringBuilder();

            string fullPath = Path.Combine(Folder, path);

            using (StreamReader sr = new StreamReader(fullPath)) {
                result = new char[sr.BaseStream.Length];

                await sr.ReadAsync(result, 0, (int)sr.BaseStream.Length);
            }

            foreach (char c in result) {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)) {
                    builder.Append(c);
                }
            }

            callback.Function.Run(Engine, null, new Arguments(new BaseObject[] { Engine.CreateString(builder.ToString()) }));
        }

        public async void WriteAsync(string destination, string content, FunctionInstance callback) {
            string fullPath = Path.Combine(Folder, destination);

            var directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter sw = new StreamWriter(fullPath)) {
                foreach (var c in content) {
                    await sw.WriteAsync(c);
                }
            }

            callback.Function.Run(Engine, null, new Arguments(null));
        }
    }
}
