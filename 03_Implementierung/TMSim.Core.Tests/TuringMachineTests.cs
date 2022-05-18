﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

namespace TMSim.Core.Tests
{
    
        
    [DeploymentItem("res/example_import.tmsim","res")]
    [DeploymentItem("res/example_export.tmsim", "res")]
    [TestClass]
    public class TuringMachineTests
    {
        [TestMethod]
        public void CheckIsEndState_ShouldReturnTrue()
        {
            Alphabet inputAlphabet = new Alphabet("a");
            Alphabet tapeAlphabet = new Alphabet("a_");
            List<TuringState> states = new List<TuringState>() { new TuringState("0"), new TuringState("1") };
            List<TuringTransition> transitions = new List<TuringTransition>() { new TuringTransition(states[0], states[1], new List<char>() { 'a' }, new List<char>() { 'a' }, new List<TuringTransition.Direction>() { TuringTransition.Direction.Right }) };
            List<TuringTape> tapes = new List<TuringTape>() { new TuringTape("aaaaa", '_') };


            TuringMachine tm = new TuringMachine(tapeAlphabet, '_', inputAlphabet, states, states[0], new List<TuringState>() { states[1] }, transitions, tapes);
            tm.AdvanceState();
            Assert.IsTrue(tm.CheckIsEndState());
        }

        [TestMethod]
        public void CheckIsEndState_ShouldReturnFalse()
        {
            Alphabet inputAlphabet = new Alphabet("a");
            Alphabet tapeAlphabet = new Alphabet("a_");
            List<TuringState> states = new List<TuringState>() { new TuringState(""), new TuringState("") };
            List<TuringTransition> transitions = new List<TuringTransition>();
            List<TuringTape> tapes = new List<TuringTape>() { new TuringTape("aaaaa", '_') };


            TuringMachine tm = new TuringMachine(tapeAlphabet, '_', inputAlphabet, states, states[0], new List<TuringState>() { states[1] }, transitions, tapes);
            Assert.IsFalse(tm.CheckIsEndState()); 
        }


        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_TapeAlphabetIs_abcde_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMachine.TapeAlphabet.WordIsContainedIn("abcde_"));
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

            TuringMachine turingMachine = new TuringMachine(bandAlphabet, blankChar, inputAlphabet, states, state, endStates, transitions, new List<TuringTape>());

            turingMachine.ImportFromTextFile(@"res/example_import.tmsim");
            Assert.IsTrue(turingMachine.BlankChar == '_');
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_InputAlphabetIs_abcde_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMachine.InputAlphabet.WordIsContainedIn("abcde"));
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_CheckStates_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();

            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");

            List<TuringState> states = new List<TuringState>();
            states.Add(new TuringState("q0"));
            states.Add(new TuringState("q1"));
            states.Add(new TuringState("q2"));
            states.Add(new TuringState("q3"));
            states.Add(new TuringState("..."));

            for (int i = 0; i < states.Count; i++)
            {
                Assert.IsTrue(states[i].Identifier == turingMachine.States[i].Identifier);
            }
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_StartStateIdentifierIs_q0_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMachine.StartState.Identifier == "q0");
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_CurrentStateIsStartState_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            Assert.IsTrue(turingMachine.CurrentState.Identifier == turingMachine.StartState.Identifier);
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_EndStatesIsCorrect_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");

            List<TuringState> testEndStates = new List<TuringState>();
            testEndStates.Add(new TuringState("q2"));
            testEndStates.Add(new TuringState("q3"));

            for (int i = 0; i < testEndStates.Count; i++)
            {
                Assert.IsTrue(testEndStates[i].Identifier == turingMachine.EndStates[i].Identifier);
            }
        }

        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_CheckTuringTransition_ReturnsTrue()
        {

            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");

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

            Assert.IsTrue(turingMachine.Transitions[0].Source.Identifier == turingTransition.Source.Identifier);
            Assert.IsTrue(turingMachine.Transitions[0].Target.Identifier == turingTransition.Target.Identifier);

            for (int i = 0; i < symbolsRead.Count; i++)
                Assert.IsTrue(turingMachine.Transitions[0].SymbolsRead[i] == turingTransition.SymbolsRead[i]);

            for (int i = 0; i < symbolsWrite.Count; i++)
                Assert.IsTrue(turingMachine.Transitions[0].SymbolsWrite[i] == turingTransition.SymbolsWrite[i]);

            for (int i = 0; i < directions.Count; i++)
                Assert.IsTrue(turingMachine.Transitions[0].MoveDirections[i] == turingTransition.MoveDirections[i]);
        }

        [TestMethod]
        public void ExportToTextFile_ContentShouldBeTheSameAfterExport() {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            turingMachine.ExportToTextFile(@"res\example_export.tmsim");
            string contentImport = System.IO.File.ReadAllText(@"res\example_import.tmsim");
            string contentExport = System.IO.File.ReadAllText(@"res\example_export.tmsim");
            Assert.IsTrue(string.Equals(contentExport, contentImport));
        }

        [TestMethod]
        public void ExportToTextFile_ExportEmptyTuringMachine()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ExportToTextFile(@"res\example_export.tmsim");
            string contentImport = System.IO.File.ReadAllText(@"res\example_export.tmsim");
            Assert.IsTrue(true);
        }
    }
}
