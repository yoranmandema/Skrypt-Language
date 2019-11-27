using Xunit;
using Skrypt;
using System;
using Xunit.Abstractions;

namespace Skrypt.Tests {
    [TestCaseOrderer("Skrypt.Tests.PriorityOrderer", "Skrypt.Tests")]
    public class StructTests {

        private readonly SkryptEngine _engine;
        private readonly ITestOutputHelper _output;

        public StructTests(ITestOutputHelper output) {
            _output = output;
            _engine = new SkryptEngine();

            _engine
                .SetValue("assert", new Action<bool>(Assert.True))
                .SetValue("equal", new Action<object, object>(Assert.Equal))
                ;

            _engine.Execute(@"
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
            ").CreateGlobals();
        }

        private void RunTest(string source) {
            _engine.Execute(source).ReportErrors().CreateGlobals();

            if (_engine.ErrorHandler.HasErrors) {
                _output.WriteLine($"Errors:");

                foreach (var err in _engine.ErrorHandler.Errors) {
                    _output.WriteLine($"({err.Line},{err.Column}) {err.Message}");
                }

                throw new FatalErrorException();
            }
        }

        [Fact, TestPriority(0)]
        public void ShouldParseStruct() {
            RunTest(@"
struct TestStruct {
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

        [Fact, TestPriority(1)]
        public void ShouldConstructStruct() {
            RunTest(@"
instance = BasicStruct(1,""Hello"")
            ");
        }
    }
}
