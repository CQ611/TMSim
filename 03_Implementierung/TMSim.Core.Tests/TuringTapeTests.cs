using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [TestClass]
    public class TuringTapeTests
    {
        [TestMethod]
        public void GetCurrentSymbol_ContentABC_ReturnsA()
        {
            TuringTape tape = new TuringTape("ABC", '_');
            char currentSymbol = tape.GetCurrentSymbol();
            Assert.IsTrue(currentSymbol == 'A');
        }

        [TestMethod]
        public void GetCurrentSymbol_ContentABCMoveRight_ReturnsB()
        {
            TuringTape tape = new TuringTape("ABC", '_');
            tape.MoveRight();
            char currentSymbol = tape.GetCurrentSymbol();
            Assert.IsTrue(currentSymbol == 'B');
        }

        [TestMethod]
        public void GetCurrentSymbol_ContentABCMoveLeft_ReturnsBlank()
        {
            TuringTape tape = new TuringTape("ABC", '_');
            tape.MoveLeft();
            char currentSymbol = tape.GetCurrentSymbol();
            Assert.IsTrue(currentSymbol == '_');
        }

        [TestMethod]
        public void GetCurrentSymbol_ContentABCMoveLeftTwoTimes_ReturnsBlank()
        {
            TuringTape tape = new TuringTape("ABC", '_');
            tape.MoveLeft();
            tape.MoveLeft();
            char currentSymbol = tape.GetCurrentSymbol();
            Assert.IsTrue(currentSymbol == '_');
        }

        [TestMethod]
        public void GetCurrentSymbol_ContentAMoveRight_ReturnsBlank()
        {
            TuringTape tape = new TuringTape("A", '_');
            tape.MoveLeft();
            tape.MoveLeft();
            char currentSymbol = tape.GetCurrentSymbol();
            Assert.IsTrue(currentSymbol == '_');
        }

        [TestMethod]
        public void GetCurrentSymbol_ContentABCMoveRightMoveLeft_ReturnsA()
        {
            TuringTape tape = new TuringTape("ABC", '_');
            tape.MoveRight();
            tape.MoveLeft();
            char currentSymbol = tape.GetCurrentSymbol();
            Assert.IsTrue(currentSymbol == 'A');
        }
    }
}
