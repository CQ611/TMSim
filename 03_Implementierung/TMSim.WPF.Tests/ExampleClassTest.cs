using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.WPF.Tests
{
    [TestClass]
    public class ExampleClassTest
    {
        [TestMethod]
        public void ReturnTrue_ReturnsTrue()
        {
            ExampleClass mtc = new();
            Assert.IsTrue(mtc.ReturnTrue);
        }
    }
}
