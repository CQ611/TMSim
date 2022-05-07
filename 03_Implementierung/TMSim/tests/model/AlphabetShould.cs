using TMSim.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.tests.model
{
    [TestClass]
    class AlphabetShould
    {
        [TestMethod]
        public void WordIsContainedIn_AlphaABCInputABBCCC_ReturnsTrue() {
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
    }
}
