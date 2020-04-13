using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Native {
    public interface IInitializeOnParse {
        bool IsInitialized { get; set; }

        void Initialize();
    }
}
