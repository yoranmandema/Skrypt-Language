using System;
using Xunit;
using Skrypt;

namespace Skrypt.Tests {
    public class LiteralTests {
        private readonly Engine _engine;
        public LiteralTests() {
            _engine = new Engine();
        }

        [Fact]
        public void ShouldParseNull() {
            var nullValue = _engine.Run("null").CompletionValue;

            Assert.Null(nullValue);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(42, "42")]
        [InlineData(0.14, "0.14")]
        [InlineData(3.14159, "3.14159")]
        public void ShouldParseNumericLiterals(object expected, string source) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(Convert.ToDouble(expected), value.AsType<NumberInstance>().Value);
        }

        [Theory]
        [InlineData("\"test\"", "test")]
        [InlineData("\"\"", "")]
        [InlineData("\"\\n\"", "\n")]
        public void ShouldParseStringLiterals(string source, string expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<StringInstance>().Value);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void ShouldParseBoolLiterals(string source, bool expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<BooleanInstance>().Value);
        }
    }
}
