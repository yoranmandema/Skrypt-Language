using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skrypt {
    public interface IFileHandler {
        Engine  Engine { get; set; }
        string  File { get; set; }
        string  Folder { get; set; }
        string  BaseFolder { get; set; }
        string  Read(string path);
        void    Write(string path, string content);
        void    ReadAsync(string path, FunctionInstance callback);
        void    WriteAsync(string path, string content, FunctionInstance callback);
    }
}

