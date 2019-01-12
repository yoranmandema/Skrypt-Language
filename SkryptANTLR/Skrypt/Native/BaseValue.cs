using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class BaseValue {
        public virtual string Name { get; set; }

        public Engine Engine { get; set; }
        public Dictionary<string, Member> Members = new Dictionary<string, Member>();

        public BaseValue(Engine engine) {
            Engine = engine;
        }

        public void GetProperties (Dictionary<string, Member> properties) {
            Members = Members.Concat(properties).ToDictionary(d => d.Key, d => d.Value);
        }

        public void GetProperties(Template template) {
            Members = Members.Concat(template.Members).ToDictionary(d => d.Key, d => d.Value);
            Name = template.Name;
        }

        public Member SetProperty (string name, BaseValue value) {
            if (!Members.ContainsKey(name)) {
                throw new NonExistingMemberException($"Value does not contain a member with the name '{name}'.");
            }

            Members[name].Value = value;

            return Members[name];
        }

        public Member CreateProperty(string name, BaseValue value, bool isPrivate = false) {
            Members[name] = new Member(value, isPrivate, null);

            return Members[name];
        }

        public Member GetProperty(string name) {
            if (!Members.ContainsKey(name)) {
                throw new NonExistingMemberException($"Value does not contain a member with the name '{name}'.");
            }

            return Members[name];
        }

        public T AsType<T>() where T : BaseValue {
            return (T)this;
        }

        public virtual bool IsTrue () {
            return true;
        }

        public BaseValue Clone() {
            return (BaseValue)MemberwiseClone();
        }

        public override string ToString() {
            var str = $"{Name}";

            foreach (var kv in Members) {
                str += $"\n{kv.Key}:\t{kv.Value.Value}";
            }

            return str;
        }
    }
}
