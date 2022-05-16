using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputABBCCC_ReturnsTrue()
        {
            Alphabet a = new Alphabet("ABC");
            bool isContained = a.WordIsContainedIn("ABBCCC");
            Assert.IsTrue(isContained);
        }


        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputABD_ReturnsFalse()
        {
            Alphabet a = new Alphabet("ABC");
            bool isContained = a.WordIsContainedIn("ABD");
            Assert.IsFalse(isContained);
        }


        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputBCA_ReturnsTrue()
        {
            Alphabet a = new Alphabet("ABC");
            bool isContained = a.WordIsContainedIn("BCA");
            Assert.IsTrue(isContained);
        }
    }
}
