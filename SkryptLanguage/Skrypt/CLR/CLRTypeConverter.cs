using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRTypeConverter {
        private static Delegate CreateDelegate(MethodInfo methodInfo, object target) {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals((typeof(void)));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction) {
                getType = Expression.GetActionType;
            }
            else {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic) {
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }

        public static CLRMethod CreateCLRFunction(MethodInfo methodInfo, object target) {
            var newCLRFunction = new CLRMethod() {
                parameters = methodInfo.GetParameters(),
                methodInfo = methodInfo
            };

            newCLRFunction.del = CreateDelegate(methodInfo, target);

            return newCLRFunction;
        }

        public static CLRMethod CreateCLRFunction(Delegate del) {
            var newCLRFunction = new CLRMethod() {
                parameters = del.Method.GetParameters(),
                methodInfo = del.Method
            };

            newCLRFunction.del = del;

            return newCLRFunction;
        }

        private static bool HasValidParameters (MethodInfo methodInfo) {
            if (methodInfo.CallingConvention == CallingConventions.VarArgs) return false;

            ParameterInfo[] parameters = methodInfo.GetParameters();

            foreach (ParameterInfo parameter in parameters) {
                if (parameter.ParameterType.IsByRef) return false;
                if (parameter.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0) return false;
            }

            return true;
        }

        public static BaseModule CreateModuleFromObject (SkryptEngine engine, Type type) {
            var module = new BaseModule(engine);
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var methodGroups = new Dictionary<string, List<MethodInfo>>();

            // Group methods together by name
            foreach (var methodInfo in methodInfos) {
                if (methodGroups.ContainsKey(methodInfo.Name)) {
                    methodGroups[methodInfo.Name].Add(methodInfo);
                } else {
                    methodGroups[methodInfo.Name] = new List<MethodInfo>(new [] { methodInfo });
                }
            }

            foreach (var kv in methodGroups) {
                var CLRMethodGroup = new CLRMethodGroup();
                var functions = new List<CLRMethod>();

                foreach (var methodInfo in kv.Value) {
                    var hasValidParameters = HasValidParameters(methodInfo);

                    if (!hasValidParameters) continue;

                    var clrFunction = CreateCLRFunction(methodInfo, type);

                    functions.Add(clrFunction);
                }

                CLRMethodGroup.Functions = functions.ToArray();

                module.CreateProperty(kv.Key, new FunctionInstance(engine, CLRMethodGroup));
            }

            return module;
        }

        private static bool IsNumber(object value) {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static int ToInt(NumberInstance number) => (int)number.Value;

        public static double ToDouble(NumberInstance number) => number.Value;

        public static float ToFloat(NumberInstance number) => (float)number.Value;

        public static NumberInstance ToNumberInstance(SkryptEngine engine, double val) => engine.CreateNumber(val);
        public static BooleanInstance ToBooleanInstance(SkryptEngine engine, bool val) => engine.CreateBoolean(val);
        public static StringInstance ToStringInstance(SkryptEngine engine, string val) => engine.CreateString(val);

        public static SkryptObject ConvertToSkryptObject(SkryptEngine engine, object value) {
            if (value == null) return null;
            
            if (IsNumber(value)) {
                return ToNumberInstance(engine, Convert.ToDouble(value));
            } else if (value.GetType() == typeof(bool)) {
                return ToBooleanInstance(engine, Convert.ToBoolean(value));
            }
            else if (value.GetType() == typeof(string)) {
                return ToStringInstance(engine, Convert.ToString(value));
            }

            return null;
        }
    }
}
