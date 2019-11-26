using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class SkryptObject {
        public virtual string Name { get; set; }

        public SkryptEngine Engine { get; set; }
        public Dictionary<string, Member> Members = new Dictionary<string, Member>();

        public SkryptObject(SkryptEngine engine) {
            Engine = engine;
        }

        public void GetProperties (Dictionary<string, Member> properties) {
            Members = Members.Concat(properties).ToDictionary(d => d.Key, d => d.Value);
        }

        public void GetProperties(Template template) {
            foreach (var kv in template.Members) {
                var key = kv.Key;
                var member = kv.Value;

                Members[key] = new Member(member.value, member.isPrivate, member.definitionBlock);
            }

            Name = template.Name;
        }

        public Member SetProperty (string name, SkryptObject value) {
            if (!Members.ContainsKey(name)) {
                throw new NonExistingMemberException($"Value does not contain a member with the name '{name}'.");
            }

            Members[name].value = value;

            return Members[name];
        }

        public Member CreateProperty(string name, SkryptObject value, bool isPrivate = false, bool isConstant = false) {
            Members[name] = new Member(value, isPrivate, null);

            Members[name].isConstant = isConstant;

            return Members[name];
        }

        public Member GetProperty(string name) {
            if (!Members.ContainsKey(name)) {
                throw new NonExistingMemberException($"Value {Name} does not contain a member with the name '{name}'.");
            }

            return Members[name];
        }

        public T AsType<T>() where T : SkryptObject {
            return (T)this;
        }

        public virtual bool IsTrue () {
            return true;
        }

        public SkryptObject Clone() {
            return (SkryptObject)MemberwiseClone();
        }

        protected string FormattedString (int depth) {
            var isContainer = this is BaseModule || this is BaseType;

            //if (!isContainer) return this.ToString();

            var indent = new string('\t',depth);
            var str = $"{Name}";

            if (Members.Any() && isContainer) {
                str += $" {{";

                foreach (var kv in Members) {
                    var objectString =
                        (kv.Value.value is BaseModule || kv.Value.value is BaseType) ?
                        $"{kv.Value.value.FormattedString(depth + 1)}" :
                        $"{kv.Key} ({kv.Value.value.Name}): {kv.Value.value}";

                    str += $"\n\t{indent}{objectString}";
                }

                str += $"\n{indent}}}\n";
            }

            return str;
        }

        public override string ToString() {
            var str = $"{Name}";

            if (Members.Any()) {
                str += " {";

                foreach (var kv in Members) {
                    str += $"\n{kv.Key}:\t{kv.Value.value}";
                }

                str += "\n}";
            }

            return FormattedString(0);
        }
    }
}
