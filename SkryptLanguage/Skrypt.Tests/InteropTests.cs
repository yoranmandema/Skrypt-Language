using Xunit;
using Skrypt;
using System;

namespace Skrypt.Tests {
    public class InteropTests {

        private readonly SkryptEngine _engine;

        public InteropTests() {
            _engine = new SkryptEngine();

            _engine
                .SetValue("assert", new Action<bool>(Assert.True))
                .SetValue("equal", new Action<object, object>(Assert.Equal))
                ;
        }

        private void RunTest(string source) {
            _engine.Execute(source).ReportErrors();

            if (_engine.ErrorHandler.HasErrors) {
                throw new FatalErrorException();
            }
        }

        [Fact]
        public void PrimitiveTypesCanBeSet() {
            _engine.SetValue("x", 10);
            _engine.SetValue("y", true);
            _engine.SetValue("z", "foo");

            RunTest(@"
                assert(x == 10)
                assert(y == true)
                assert(z == ""foo"")
            ");
        }


    }
}