using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [TestClass]
    public class TuringMachineTests
    {
        [TestMethod]
        public void CheckIsEndState_ShouldReturnTrue()
        {
            Alphabet inputAlphabet = new Alphabet("a");
            Alphabet tapeAlphabet = new Alphabet("a_");
            List<TuringState> states = new List<TuringState>() { new TuringState(), new TuringState() };
            List<TuringTransition> transitions = new List<TuringTransition>(){new TuringTransition(states[0], states[1], new List<char>(){'a'}, new List<char>(){'a'}, new List<TuringTransition.Direction>(){TuringTransition.Direction.Right})};
            List<TuringTape> tapes = new List<TuringTape>(){ new TuringTape("aaaaa", '_')};


            TuringMaschine tm = new TuringMaschine(tapeAlphabet, '_', inputAlphabet, states, states[0], new List<TuringState>() { states[1] }, transitions, tapes);
            tm.AdvanceState();
            Assert.IsTrue(tm.CheckIsEndState());
        }

        [TestMethod]
        public void CheckIsEndState_ShouldReturnFalse()
        {
            Alphabet inputAlphabet = new Alphabet("a");
            Alphabet tapeAlphabet = new Alphabet("a_");
            List<TuringState> states = new List<TuringState>() { new TuringState(), new TuringState() };
            List<TuringTransition> transitions = new List<TuringTransition>();
            List<TuringTape> tapes = new List<TuringTape>(){ new TuringTape("aaaaa", '_')};


            TuringMaschine tm = new TuringMaschine(tapeAlphabet, '_', inputAlphabet, states, states[0], new List<TuringState>() { states[1] }, transitions, tapes);
            Assert.IsFalse(tm.CheckIsEndState());
        }
    }
}
