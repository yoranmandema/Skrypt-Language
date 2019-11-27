using Xunit;
using Skrypt;

namespace Skrypt.Tests {
    public class AssignTests {

        private readonly SkryptEngine _engine;

        private class BasicStruct : SkryptObject {
            public BasicStruct(SkryptEngine engine) : base(engine) {
                CreateProperty("Property", null);
            }
        }


        public AssignTests () {
            _engine = new SkryptEngine();

            _engine.SetValue("BasicStruct", new BasicStruct(_engine));
            _engine.SetValue("numberVariable", 5);
        }

        [Fact]
        public void ShouldAssignVariable() {
            var value = _engine.Execute("a = 1").GetValue("a");

            Assert.NotNull(value);
            Assert.Equal(1, value.AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ShouldAssignIndex() {
            _engine.Execute("a = [0,0,0]");

            var value = _engine.Execute("a[1] = 1").GetValue("a");

            Assert.NotNull(value);
            Assert.Equal(1, value.AsType<ArrayInstance>().SequenceValues[1].AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ShouldAssignMember() {
            var value = _engine.Execute("BasicStruct.Property = 1").GetValue("BasicStruct");

            Assert.NotNull(value);
            Assert.Equal(1, value.GetProperty("Property").value.AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ShouldDoAssignWithOperator() {
            var value = _engine.Execute("numberVariable += 2").GetValue("numberVariable");

            Assert.NotNull(value);
            Assert.Equal(7, value.AsType<NumberInstance>().Value);
        }
    }
}
