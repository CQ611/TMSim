using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.UI.Tests
{
    [TestClass]
    public class ExampleClassTest
    {
        [TestMethod]
        public void ReturnTrue_ReturnsTrue()
        {
            var mtc = new ExampleClass();
            Assert.IsTrue(mtc.ReturnTrue);
        }
    }
}
