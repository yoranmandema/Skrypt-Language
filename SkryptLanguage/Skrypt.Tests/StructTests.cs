using Xunit;
using Skrypt;
using System;

namespace Skrypt.Tests {
    public class StructTests {

        private readonly SkryptEngine _engine;
        public StructTests() {
            _engine = new SkryptEngine();

            _engine
                .SetValue("assert", new Action<bool>(Assert.True))
                .SetValue("equal", new Action<object, object>(Assert.Equal))
                ;
        }

        private void RunTest(string source) {
            _engine.Run(source).CreateGlobals().ReportErrors();

            if (_engine.ErrorHandler.HasErrors) {
                throw new FatalErrorException();
            }
        }

        [Fact]
        public void ShouldParseStruct() {
            RunTest(@"
struct BasicStruct {
    A = 0
    B = """"

    fn init (a,b) {
        self.A = a
        self.B = b
    }

    fn toString () {
        return ""{"" + self.A + "","" + self.B + ""}""
    }
}
            ");
        }

        [Fact]
        public void ShouldConstructStruct() {
            RunTest(@"
instance = BasicStruct(1,""Hello"")
            ");
        }
    }
}
