using Skrypt.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class SkryptModule : SkryptObject, IInitializeOnParse {


        public SkryptModule(SkryptEngine engine) : base(engine) {
            var template = engine.TemplateMaker.CreateTemplate(this.GetType());

            GetProperties(template.Members);

            Name = template.Name;
        }

        public bool IsInitialized { get; set; }

        public void Initialize() {
            IsInitialized = true;

            OnInitialize();
        }

        protected virtual void OnInitialize () {

        }
    }
}
