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
    public A = 0
    public B = """"

    public fn init (a,b) {
        self.A = a
        self.B = b
    }

    public fn toString () {
        return ""{"" + self.A + "","" + self.B + ""}""
    }
}
            ");
        }

        private void RunTest(string source) {
            _engine.Execute(source, new Compiling.ParserOptions {
                Tolerant = false
            });
        }

        [Fact, TestPriority(0)]
        public void ShouldParseStruct() {
            RunTest(@"
struct TestStruct {
    public A = 0
    public B = """"

    public fn init (a,b) {
        self.A = a
        self.B = b
    }

    public fn toString () {
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
