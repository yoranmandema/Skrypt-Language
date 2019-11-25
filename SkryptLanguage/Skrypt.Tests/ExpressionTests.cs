using Xunit;
using Skrypt;

namespace Skrypt.Tests {
    public class ExpressionTests {

        private readonly Engine _engine;
        public ExpressionTests() {
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

        [Theory]
        [InlineData("1 + 1", 2)]
        [InlineData("2 * 4", 8)]
        [InlineData("2 * 4 + 2", 10)]
        [InlineData("2 + 2 * 4", 10)]
        [InlineData("15 / 3", 5)]
        [InlineData("15 / 3 + 2", 7)]
        [InlineData("2 + 15 / 3", 7)]
        [InlineData("(2 + 13) * 2", 30)]
        [InlineData("-30", -30)]
        public void ShouldEvaluateNumericExpressions(string source, double expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<NumberInstance>().Value);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("true && true", true)]
        [InlineData("true && false", false)]
        [InlineData("false && true", false)]
        [InlineData("false && false", false)]
        [InlineData("true || true", true)]
        [InlineData("true || false", true)]
        [InlineData("false || true", true)]
        [InlineData("false || false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]
        public void ShouldEvaluateBooleanExpressions(string source, bool expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<BooleanInstance>().Value);
        }

        [Theory]
        [InlineData("true ? 1 : 0", 1)]
        [InlineData("false ? 1 : 0", 0)]
        [InlineData("1 > 0 ? 1 : 0", 1)]
        public void ShouldEvaluateConditional(string source, double expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<NumberInstance>().Value);
        }

        [Theory]
        [InlineData("(String(1)).Length", 1)]
        public void ShouldEvaluateMemberAccessAfterParenthesis (string source, double expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<NumberInstance>().Value);
        }

        [Theory]
        [InlineData("1 + 3", 4d)]
        [InlineData("1 - 3", -2d)]
        [InlineData("1 * 3", 3d)]
        [InlineData("6 / 3", 2d)]
        [InlineData("15 & 9", 9d)]
        [InlineData("15 | 9", 15d)]
        [InlineData("15 ^ 9", 6d)]
        [InlineData("9 << 2", 36d)]
        [InlineData("9 >> 2", 2d)]
        [InlineData("19 >>> 2", 4d)]
        public void ShouldEvaluateBinaryExpression(string source, double expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.Equal(expected, value.AsType<NumberInstance>().Value);
        }

        [Theory]
        [InlineData("~58", -59d)]
        [InlineData("~~58", 58d)]
        public void ShouldInterpretUnaryExpression(string source, double expected) {
            var value = _engine.Run(source).CompletionValue;

            Assert.Equal(expected, value.AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ArrowFunctionCall() {
            RunTest(@"
                add = (a, b) => {
                    return a + b
                }
                x = add(1, 2)
                assert(x == 3)
            ");
        }
    }
}
