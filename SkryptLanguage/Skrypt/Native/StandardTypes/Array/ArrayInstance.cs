using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ArrayInstance : SkryptInstance {
        public List<SkryptObject> SequenceValues = new List<SkryptObject>();
        public Dictionary<SkryptObject,SkryptObject> Dictionary = new Dictionary<SkryptObject, SkryptObject>();

        public ArrayInstance(SkryptEngine engine) : base(engine) {
            CreateProperty("iteratorIndex", engine.CreateNumber(0), true);
        }

        public SkryptObject Get (int index) {
            if (index >= 0 && index < SequenceValues.Count) {
                return SequenceValues[index];
            } else {
                return null;
            }
        }

        public SkryptObject Get(SkryptObject index) {
            if (index is NumberInstance number && number >= 0 && number % 1 == 0) {
                return Get((int)number);
            }

            else if (index is IValue val) {
                foreach (var key in Dictionary.Keys) {
                    if (key is IValue val2 && val2.Equals(val)) {
                        return Dictionary[key];
                    }
                }
            }

            if (Dictionary.ContainsKey(index)) {
                return Dictionary[index];
            }

            return null;
        }

        public SkryptObject Set(SkryptObject index, SkryptObject value) {
            if (index is NumberInstance number && number >= 0 && number % 1 == 0) {
                if (number >= 0 && number < SequenceValues.Count) {
                    SequenceValues[(int)number] = value;
                }
                else if (number >= SequenceValues.Count) {
                    var diff = number - SequenceValues.Count + 1;

                    for (int i = 0; i < diff; i++) {
                        SequenceValues.Add(null);
                    }

                    SequenceValues[(int)number] = value;
                }
            }
            else {
                Dictionary[index] = value;
            }

            return this;
        }

        public static SkryptObject Length(SkryptEngine engine, SkryptObject self) {
            return engine.CreateNumber((self as ArrayInstance).SequenceValues.Count);
        }

        public static SkryptObject Push(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            foreach (var a in arguments.Values) {
                (self as ArrayInstance).SequenceValues.Add(a);
            }

            return null;
        }

        public static SkryptObject Concat(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var otherArray = arguments.GetAs<ArrayInstance>(0);
            var newArray = engine.CreateArray(new SkryptObject[0]);

            foreach (var v in (self as ArrayInstance).SequenceValues) {
                newArray.SequenceValues.Add(v);
            }

            foreach (var v in otherArray.SequenceValues) {
                newArray.SequenceValues.Add(v);
            }

            return newArray;
        }

        public static SkryptObject Map(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var function = arguments.GetAs<FunctionInstance>(0);

            for (int i = 0; i < array.SequenceValues.Count; i++) {
                SkryptObject functionResult = null;

                if (function.Function is ScriptFunction scriptFunction) {
                    functionResult = function.Run(array.SequenceValues[i], engine.CreateNumber(i));
                }
                else {
                    functionResult = function.Run(array.SequenceValues[i]);
                }

                array.SequenceValues[i] = functionResult;
            }

            return array;
        }

        public static SkryptObject ForEach(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var function = arguments.GetAs<FunctionInstance>(0);

            for (int i = 0; i < array.SequenceValues.Count; i++) {
                if (function.Function is ScriptFunction scriptFunction) {
                    function.Run(array.SequenceValues[i], engine.CreateNumber(i));
                }
                else {
                    function.Run(array.SequenceValues[i]);
                }
            }

            return array;
        }

        public static SkryptObject Insert(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var index = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(0)),0), array.SequenceValues.Count);

            if (arguments.Values.Length > 1) {
                for (int i = arguments.Values.Length - 1; i > 0; i--) {
                    array.SequenceValues.Insert(index, arguments.Values[i]);
                }
            }

            return null;
        }

        public static SkryptObject Slice(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var start = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(0)), 0), array.SequenceValues.Count);

            var endNumber = arguments[1];
            var end = array.SequenceValues.Count;

            if (endNumber != null) {
                end = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(1)), 0), array.SequenceValues.Count);
            }

            var newArray = engine.CreateArray(new SkryptObject[0]);

            for (int i = start; i < end; i++) {
                newArray.SequenceValues.Add(array.SequenceValues[i]);
            }

            return newArray;
        }

        public static SkryptObject Remove(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var toRemove = arguments.GetAs<SkryptObject>(0);

            if (toRemove is NumberInstance) {
                array.SequenceValues.RemoveAt((int)(toRemove as NumberInstance).Value);
            }  else { 
                var found = array.SequenceValues.Find(x => x == toRemove);

                if (found != null) {
                    array.SequenceValues.Remove(found);
                }
            }

            return null;
        }

        public override string ToString() {
            return $"[{string.Join(",", SequenceValues)}]";
        }
    }
}
