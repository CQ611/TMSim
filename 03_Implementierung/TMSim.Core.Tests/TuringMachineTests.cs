using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

namespace TMSim.Core.Tests
{
    [TestClass]
    [DeploymentItem("res/example_import.xml","res")]
    public class TuringMachineTests
    {
        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_TapeAlphabetIs_abcde_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();
            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMaschine.TapeAlphabet.WordIsContainedIn("abcde_"));
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_BlankCharIsUnderscore_ReturnsTrue()
        {

            Alphabet inputAlphabet = new Alphabet("a");
            Alphabet bandAlphabet = new Alphabet("ab#");
            char blankChar = '#';
            TuringState state = new TuringState("aus test");
            List<TuringState> states = new List<TuringState>();
            states.Add(state);
            TuringState endState = new TuringState("test123");
            List<TuringState> endStates = new List<TuringState>();
            endStates.Add(endState);
            TuringTransition transition = new TuringTransition(state, endState, new List<char>(), new List<char>(), new List<TuringTransition.Direction>());
            List<TuringTransition> transitions = new List<TuringTransition>();
            transitions.Add(transition);

            TuringMaschine turingMaschine = new TuringMaschine(bandAlphabet, blankChar, inputAlphabet, states, state, endStates, transitions, new List<TuringTape>());

            turingMaschine.ImportFromTextFile(@"res/example_import.tmsim");
            Assert.IsTrue(turingMaschine.BlankChar == '_');
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_InputAlphabetIs_abcde_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();
            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMaschine.InputAlphabet.WordIsContainedIn("abcde"));
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_CheckStates_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();

            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");

            List<TuringState> states = new List<TuringState>();
            states.Add(new TuringState("q0"));
            states.Add(new TuringState("q1"));
            states.Add(new TuringState("q2"));
            states.Add(new TuringState("q3"));
            states.Add(new TuringState("..."));

            for (int i = 0; i < states.Count; i++)
            {
                Assert.IsTrue(states[i].Identifier == turingMaschine.States[i].Identifier);
            }
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_StartStateIdentifierIs_q0_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();
            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMaschine.StartState.Identifier == "q0");
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_CurrentStateIsStartState_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();
            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMaschine.CurrentState.Identifier == turingMaschine.StartState.Identifier);
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_EndStatesIsCorrect_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();
            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");

            List<TuringState> testEndStates = new List<TuringState>();
            testEndStates.Add(new TuringState("q2"));
            testEndStates.Add(new TuringState("q3"));

            for (int i = 0; i < testEndStates.Count; i++)
            {
                Assert.IsTrue(testEndStates[i].Identifier == turingMaschine.EndStates[i].Identifier);
            }
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_CheckTuringTransition_ReturnsTrue()
        {

            TuringMaschine turingMaschine = new TuringMaschine();
            turingMaschine.ImportFromTextFile(@"res\example_import.tmsim");

            List<char> symbolsRead = new List<char>();
            symbolsRead.Add('a');
            symbolsRead.Add('b');

            List<char> symbolsWrite = new List<char>();
            symbolsWrite.Add('b');
            symbolsWrite.Add('a');

            List<TuringTransition.Direction> directions = new List<TuringTransition.Direction>();
            directions.Add(TuringTransition.Direction.Right);
            directions.Add(TuringTransition.Direction.Left);

            TuringTransition turingTransition = new TuringTransition(new TuringState("q0"), new TuringState("q1"), symbolsRead, symbolsWrite, directions);

            Assert.IsTrue(turingMaschine.Transitions[0].Source.Identifier == turingTransition.Source.Identifier);
            Assert.IsTrue(turingMaschine.Transitions[0].Target.Identifier == turingTransition.Target.Identifier);

            for (int i = 0; i < symbolsRead.Count; i++)
                Assert.IsTrue(turingMaschine.Transitions[0].SymbolsRead[i] == turingTransition.SymbolsRead[i]);

            for (int i = 0; i < symbolsWrite.Count; i++)
                Assert.IsTrue(turingMaschine.Transitions[0].SymbolsWrite[i] == turingTransition.SymbolsWrite[i]);

            for (int i = 0; i < directions.Count; i++)
                Assert.IsTrue(turingMaschine.Transitions[0].MoveDirections[i] == turingTransition.MoveDirections[i]);
        }
    }
}
