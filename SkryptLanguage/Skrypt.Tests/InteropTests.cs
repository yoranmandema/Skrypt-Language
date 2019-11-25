using Xunit;
using Skrypt;

namespace Skrypt.Tests {
    public class InteropTests {

        private readonly Engine _engine;

        public InteropTests() {
            _engine = new Engine();

            _engine
                .SetValue("assert", (e, s, i) => { Assert.True(i.GetAs<BooleanInstance>(0).Value); return null; })
                .SetValue("equal", (e, s, i) => { Assert.Equal(i[0], i[1]); return null; })
                ;
        }

        private void RunTest(string source) {
            _engine.Run(source).ReportErrors();

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