using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public abstract class SkryptType : SkryptObject {
        public SkryptType(SkryptEngine engine) : base(engine) {
            Template = engine.TemplateMaker.CreateTemplate(this.GetType());

            GetProperties(Template.Members);

            Name = Template.Name;
        }

        public Dictionary<string, Member> ImplementTrait (SkryptTrait trait) {
            var dict = new Dictionary<string, Member>();

            Traits.Add(trait);

            foreach (var kv in trait.TraitMembers) {
                var newMember = new Member(kv.Value.value, kv.Value.isPrivate, kv.Value.definitionBlock);

                Template.Members[kv.Key] = newMember;
                dict[kv.Key] = newMember;
            }

            return dict;
        }

        public Template Template;
        public List<SkryptTrait> Traits = new List<SkryptTrait>();
        public abstract SkryptInstance Construct(Arguments arguments);

        public override string ToString() {
            var str = $"{Name}";
            str += "\nTraits: ";

            if (Traits.Any()) {
                foreach (var t in Traits) {
                    str += $"{t.Name} ";
                }
            } else {
                str += "none";
            }

            foreach (var kv in Members) {
                str += $"\n{kv.Key}:\t{kv.Value.value}";
            }

            return str;
        }
    }
}
