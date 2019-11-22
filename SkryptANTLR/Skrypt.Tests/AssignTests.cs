using Xunit;
using Skrypt;

namespace Skrypt.Tests {
    public class AssignTests {

        private readonly Engine _engine;

        private class BasicStruct : BaseObject {
            public BasicStruct(Engine engine) : base(engine) {
                CreateProperty("Property", null);
            }
        }


        public AssignTests () {
            _engine = new Engine();

            _engine.SetValue("BasicStruct", new BasicStruct(_engine));
            _engine.SetValue("numberVariable", 5);
        }

        [Fact]
        public void ShouldAssignVariable() {
            var value = _engine.Run("a = 1").CreateGlobals().GetValue("a");

            Assert.NotNull(value);
            Assert.Equal(1, value.AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ShouldAssignIndex() {
            _engine.Run("a = [0,0,0]").CreateGlobals();

            var value = _engine.Run("a[1] = 1").GetValue("a");

            Assert.NotNull(value);
            Assert.Equal(1, value.AsType<ArrayInstance>().SequenceValues[1].AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ShouldAssignMember() {
            var value = _engine.Run("BasicStruct.Property = 1").GetValue("BasicStruct");

            Assert.NotNull(value);
            Assert.Equal(1, value.GetProperty("Property").value.AsType<NumberInstance>().Value);
        }

        [Fact]
        public void ShouldDoAssignWithOperator() {
            var value = _engine.Run("numberVariable += 2").GetValue("numberVariable");

            Assert.NotNull(value);
            Assert.Equal(7, value.AsType<NumberInstance>().Value);
        }
    }
}
