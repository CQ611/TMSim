using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void AddState_NoStartOrEndState_ShouldAddStateToStates() 
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("q0");
            tm.AddState(ts);
            Assert.IsTrue(tm.States.Contains(ts));
            Assert.IsFalse(tm.EndStates.Contains(ts));
            Assert.IsFalse(tm.StartState == ts);
        }

        [TestMethod]
        public void AddState_NoStartButEndState_ShouldAddStateToStatesAndEndStates()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("q0", isAccepting:true);
            tm.AddState(ts);
            Assert.IsTrue(tm.States.Contains(ts));
            Assert.IsTrue(tm.EndStates.Contains(ts));
            Assert.IsFalse(tm.StartState == ts);
        }

        [TestMethod]
        public void AddState_StartAndEndState_ShouldAddStateToStatesEndStatesAndStartState()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("q0", isAccepting: true, isStart:true);
            tm.AddState(ts);
            Assert.IsTrue(tm.States.Contains(ts));
            Assert.IsTrue(tm.EndStates.Contains(ts));
            Assert.IsTrue(tm.StartState == ts);
        }

        [TestMethod]
        public void AddState_AddTwoStartStates_SecondOneShouldBeSetAsStartState()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("q0", isStart: true);
            TuringState ts2 = new TuringState("q1", isStart: true);
            tm.AddState(ts1);
            tm.AddState(ts2);
            Assert.IsTrue(tm.States.Contains(ts1));
            Assert.IsTrue(tm.States.Contains(ts2));
            Assert.IsTrue(tm.StartState == ts2);
        }

        [TestMethod]
        public void EditState_ReplaceOldStateWithNewOne() {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("q0");
            TuringState ts2 = new TuringState("q1");
            tm.AddState(ts1);
            tm.EditState(ts1, ts2);
            Assert.IsFalse(tm.States.Contains(ts1));
            Assert.IsTrue(tm.States.Contains(ts2));
        }

        [TestMethod]
        public void RemoveState_AllTransitionsContainingThisStateShouldBeRemoved()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("q0");
            TuringState ts2 = new TuringState("q1");
            TuringState ts3 = new TuringState("q2");
            tm.AddState(ts1);
            tm.AddState(ts2);
            tm.AddState(ts3);
            TuringTransition tt1 = new TuringTransition(
                ts1, 
                ts2, 
                new List<char> { 'a' }, 
                new List<char> { 'a' }, 
                new List<TuringTransition.Direction> { TuringTransition.Direction.Right }
            );
            TuringTransition tt2 = new TuringTransition(
                ts2,
                ts3,
                new List<char> { 'a' },
                new List<char> { 'a' },
                new List<TuringTransition.Direction> { TuringTransition.Direction.Right }
            );
            tm.AddTransition(tt1);
            tm.AddTransition(tt2);
            tm.RemoveState(ts2);
            Assert.IsTrue(tm.States.Contains(ts1));
            Assert.IsFalse(tm.States.Contains(ts2));
            Assert.IsTrue(tm.States.Contains(ts3));
            Assert.IsTrue(tm.Transitions.Count == 0);
            Assert.IsTrue(ts1.OutgoingTransitions.Count == 0);
            Assert.IsTrue(ts3.IncomingTransitions.Count == 0);
        }

        [TestMethod]
        public void AddTransition_TransitonIsAddToTransitionsIncomingTransitonsOfTargetAndOutgoingTransitionsOfSource() 
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("q0");
            TuringState ts2 = new TuringState("q1");
            tm.AddState(ts1);
            tm.AddState(ts2);
            TuringTransition tt1 = new TuringTransition(
                ts1,
                ts2,
                new List<char> { 'a' },
                new List<char> { 'a' },
                new List<TuringTransition.Direction> { TuringTransition.Direction.Right }
            );
            tm.AddTransition(tt1);
            Assert.IsTrue(tm.Transitions.Contains(tt1));
            Assert.IsTrue(ts1.OutgoingTransitions.Contains(tt1));
            Assert.IsTrue(ts2.IncomingTransitions.Contains(tt1));
        }

        [TestMethod]
        public void RemoveTransition_TransitonIsRemovedFromTransitionsIncomingTransitonsOfTargetAndOutgoingTransitionsOfSource()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("q0");
            TuringState ts2 = new TuringState("q1");
            tm.AddState(ts1);
            tm.AddState(ts2);
            TuringTransition tt1 = new TuringTransition(
                ts1,
                ts2,
                new List<char> { 'a' },
                new List<char> { 'a' },
                new List<TuringTransition.Direction> { TuringTransition.Direction.Right }
            );
            tm.AddTransition(tt1);
            tm.RemoveTransition(tt1);
            Assert.IsFalse(tm.Transitions.Contains(tt1));
            Assert.IsFalse(ts1.OutgoingTransitions.Contains(tt1));
            Assert.IsFalse(ts2.IncomingTransitions.Contains(tt1));
        }

        [TestMethod]
        public void EditTransition_TransitonIsReplacedByNewOne()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("q0");
            TuringState ts2 = new TuringState("q1");
            tm.AddState(ts1);
            tm.AddState(ts2);
            TuringTransition tt1 = new TuringTransition(
                ts1,
                ts2,
                new List<char> { 'a' },
                new List<char> { 'a' },
                new List<TuringTransition.Direction> { TuringTransition.Direction.Right }
            );
            TuringTransition tt2 = new TuringTransition(
                ts1,
                ts2,
                new List<char> { 'b' },
                new List<char> { 'b' },
                new List<TuringTransition.Direction> { TuringTransition.Direction.Right }
            );
            tm.AddTransition(tt1);
            tm.EditTransition(tt1, tt2);
            Assert.IsFalse(tm.Transitions.Contains(tt1));
            Assert.IsFalse(ts1.OutgoingTransitions.Contains(tt1));
            Assert.IsFalse(ts2.IncomingTransitions.Contains(tt1));
            Assert.IsTrue(tm.Transitions.Contains(tt2));
            Assert.IsTrue(ts1.OutgoingTransitions.Contains(tt2));
            Assert.IsTrue(ts2.IncomingTransitions.Contains(tt2));
        }

        [TestMethod]
        public void AddSymbol_symbolIsAddedToInputAndTapeAlphabet() {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            Assert.IsTrue(tm.InputSymbols.Contains('a'));
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
        }

        [TestMethod]
        public void AddSymbol_symbolIsAddedToInputAlphabet()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', false);
            Assert.IsFalse(tm.InputSymbols.Contains('a'));
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
        }

        [TestMethod]
        public void EditSymbol_symbolIsAddedToInputAlphabet()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', false);
            Assert.IsFalse(tm.InputSymbols.Contains('a'));
            tm.EditSymbol('a', true);
            Assert.IsTrue(tm.InputSymbols.Contains('a'));
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
        }

        [TestMethod]
        public void EditSymbol_symbolIsRemovedFromInputAlphabet()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            Assert.IsTrue(tm.InputSymbols.Contains('a'));
            tm.EditSymbol('a', false);
            Assert.IsFalse(tm.InputSymbols.Contains('a'));
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
        }

        [TestMethod]
        public void RemoveSymbol_symbolIsRemovedFromInputAndTapeAlphabet()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            Assert.IsTrue(tm.InputSymbols.Contains('a'));
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
            tm.RemoveSymbol('a');
            Assert.IsFalse(tm.InputSymbols.Contains('a'));
            Assert.IsFalse(tm.TapeSymbols.Contains('a'));
        }

        [TestMethod]
        public void RemoveSymbol_AllTransitionsContainingTheSymbolAreRemoved()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            TuringState q0 = new TuringState("q0");
            TuringTransition tt = new TuringTransition(
                q0, 
                q0, 
                new List<char> { 'a' }, 
                new List<char> { 'a' }, 
                new List<TuringTransition.Direction> { TuringTransition.Direction.Left }
            );
            tm.AddState(q0);
            tm.AddTransition(tt);
            tm.RemoveSymbol('a');
            Assert.IsFalse(tm.Transitions.Contains(tt));
            Assert.IsFalse(tm.States[0].IncomingTransitions.Contains(tt));
            Assert.IsFalse(tm.States[0].OutgoingTransitions.Contains(tt));
        }

        [TestMethod]
        public void WriteTapeWord_HelloShouldBeWrittenToTape()
        {
            TuringMachine tm = new TuringMachine();
            tm.WriteTapeWord("Hello");
            Assert.IsTrue(tm.Tapes[0].Content == "Hello");
        }


        [TestMethod]
        public void ImportFromTextFile_ReadExampleFile_BlankCharIsUnderscore_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();

            turingMachine.ImportFromTextFile(@"res/example_import.tmsim");
            Assert.IsTrue(turingMachine.BlankChar == '_');
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
        public void ExportToTextFile_ContentShouldBeTheSameAfterExport()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            turingMachine.ExportToTextFile(@"res\example_export.tmsim");
            string contentImport = System.IO.File.ReadAllText(@"res\example_import.tmsim");
            string contentExport = System.IO.File.ReadAllText(@"res\example_export.tmsim");
            Assert.IsTrue(string.Equals(contentExport, contentImport));
        }
    }
}
