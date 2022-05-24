using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputABBCCC_ReturnsTrue()
        {
            TuringAlphabet a = new TuringAlphabet("ABC");
            bool isContained = a.WordIsContainedIn("ABBCCC");
            Assert.IsTrue(isContained);
        }


        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputABD_ReturnsFalse()
        {
            TuringAlphabet a = new TuringAlphabet("ABC");
            bool isContained = a.WordIsContainedIn("ABD");
            Assert.IsFalse(isContained);
        }


        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputBCA_ReturnsTrue()
        {
            TuringAlphabet a = new TuringAlphabet("ABC");
            bool isContained = a.WordIsContainedIn("BCA");
            Assert.IsTrue(isContained);
        }
    }
}
