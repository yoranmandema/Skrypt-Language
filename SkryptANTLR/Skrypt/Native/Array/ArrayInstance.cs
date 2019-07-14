using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ArrayInstance : BaseInstance {
        public List<BaseObject> SequenceValues = new List<BaseObject>();
        public Dictionary<BaseObject,BaseObject> Dictionary = new Dictionary<BaseObject, BaseObject>();

        public ArrayInstance(Engine engine) : base(engine) {
            CreateProperty("iteratorIndex", engine.CreateNumber(0), true);
        }

        public BaseObject Get (int index) {
            if (index >= 0 && index < SequenceValues.Count) {
                return SequenceValues[index];
            } else {
                return null;
            }
        }

        public BaseObject Get(BaseObject index) {
            if (index is NumberInstance number && number >= 0 && number % 1 == 0) {
                return Get((int)number);
            }

            if (Dictionary.ContainsKey(index)) {
                return Dictionary[index];
            }

            return null;
        }

        public BaseObject Set(BaseObject index, BaseObject value) {
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

        public static BaseObject Length(Engine engine, BaseObject self) {
            return engine.CreateNumber((self as ArrayInstance).SequenceValues.Count);
        }

        public static BaseObject Push(Engine engine, BaseObject self, Arguments arguments) {
            foreach (var a in arguments.Values) {
                (self as ArrayInstance).SequenceValues.Add(a);
            }

            return null;
        }

        public static BaseObject Concat(Engine engine, BaseObject self, Arguments arguments) {
            var otherArray = arguments.GetAs<ArrayInstance>(0);
            var newArray = engine.CreateArray(new BaseObject[0]);

            Console.WriteLine("Concat input" + (self as ArrayInstance) + " " + otherArray);

            foreach (var v in (self as ArrayInstance).SequenceValues) {
                newArray.SequenceValues.Add(v);
            }

            foreach (var v in otherArray.SequenceValues) {
                newArray.SequenceValues.Add(v);
            }

            Console.WriteLine("Concat result: " + newArray);

            return newArray;
        }

        public static BaseObject Insert(Engine engine, BaseObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var index = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(0)),0), array.SequenceValues.Count);

            if (arguments.Values.Length > 1) {
                for (int i = arguments.Values.Length - 1; i > 0; i--) {
                    array.SequenceValues.Insert(index, arguments.Values[i]);
                }
            }

            return null;
        }

        public static BaseObject Slice(Engine engine, BaseObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var start = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(0)), 0), array.SequenceValues.Count);

            var endNumber = arguments[1];
            var end = array.SequenceValues.Count;

            if (endNumber != null) {
                end = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(1)), 0), array.SequenceValues.Count);
            }

            var newArray = engine.CreateArray(new BaseObject[0]);

            for (int i = start; i < end; i++) {
                newArray.SequenceValues.Add(array.SequenceValues[i]);
            }

            return newArray;
        }

        public static BaseObject Remove(Engine engine, BaseObject self, Arguments arguments) {
            var array = self as ArrayInstance;
            var toRemove = arguments.GetAs<BaseObject>(0);

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
