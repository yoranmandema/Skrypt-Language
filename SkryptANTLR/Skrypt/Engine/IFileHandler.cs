using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skrypt {
    public interface IFileHandler {
        string  File { get; set; }
        string  Folder { get; set; }
        string  Read(string path);
        void    Write(string path, string content);
    }
}
