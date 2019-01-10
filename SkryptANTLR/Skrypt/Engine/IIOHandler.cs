using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skrypt {
    public interface IIOHandler {
        string File { get; set; }
        string Directory { get; set; }
        string Read(string path);
    }
}
