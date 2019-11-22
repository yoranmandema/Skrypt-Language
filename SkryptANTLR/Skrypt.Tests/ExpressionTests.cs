using Xunit;
using Skrypt;

namespace Skrypt.Tests {
    public class ExpressionTests {
        [Theory]
        [InlineData("1 + 1", 2)]
        [InlineData("2 * 4", 8)]
        [InlineData("2 * 4 + 2", 10)]
        [InlineData("2 + 2 * 4", 10)]
        [InlineData("15 / 3", 5)]
        [InlineData("15 / 3 + 2", 7)]
        [InlineData("2 + 15 / 3", 7)]
        [InlineData("(2 + 13) * 2", 30)]
        public void ShouldEvaluateNumericExpressions(string source, double expected) {
            var value = new Engine().Run(source).CompletionValue;

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
            var value = new Engine().Run(source).CompletionValue;

            Assert.NotNull(value);
            Assert.Equal(expected, value.AsType<BooleanInstance>().Value);
        }
    }
}
