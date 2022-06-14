using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using TMSim.Core.Exceptions;

namespace TMSim.Core.Tests
{
    
        
    [DeploymentItem("res/example_import.tmsim","res")]
    [DeploymentItem("res/example_export.tmsim", "res")]
    [DeploymentItem("res/example_invalid.tmsim", "res")]
    [DeploymentItem("res/example_input_alphabet_is_no_subset_of_tape_alphabet.tmsim", "res")]
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
            Assert.IsTrue(tm.States.Contains(ts1));
            Assert.IsFalse(tm.States.Contains(ts2));
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
            tm.AddSymbol('a', true);
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
            tm.AddSymbol('a', true);
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
            tm.AddSymbol('a', true);
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
            tm.AddSymbol('a', true);
            tm.AddSymbol('b', true);
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
        public void AddSymbol_symbolIsSetAsBlankCharAndIsAddedToTapeAlphabet()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', false, true);
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
            Assert.IsTrue(tm.BlankChar == 'a');
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
        public void EditSymbol_symbolIsSetAsBlankChar()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            Assert.IsTrue(tm.InputSymbols.Contains('a'));
            tm.EditSymbol('a', false, true);
            Assert.IsFalse(tm.InputSymbols.Contains('a'));
            Assert.IsTrue(tm.TapeSymbols.Contains('a'));
            Assert.IsTrue(tm.BlankChar == 'a');
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
        public void RemoveSymbol_BlankCharIsSetToNullWhenBlankCharIsRemoved()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('#', false, true);
            Assert.IsTrue(tm.BlankChar == '#');
            tm.RemoveSymbol('#');
            Assert.IsTrue(tm.BlankChar == '\0');
            Assert.IsTrue(tm.Tapes[0].GetCurrentSymbol() == '\0');
        }

        [TestMethod]
        public void WriteTapeWord_HelloShouldBeWrittenToTape()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('H', true);
            tm.AddSymbol('e', true);
            tm.AddSymbol('l', true);
            tm.AddSymbol('o', true);
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
        [ExpectedException(typeof(ImportFileIsNotValidException))]
        public void ImportFromTextFile_throwsImportFileIsNotValidException()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_invalid.tmsim");
        }

        [TestMethod]
        [ExpectedException(typeof(InputAlphabetHasToBeASubsetOfTapeAlphabetException))]
        public void ImportFromTextFile_throwsInputAlphabetHasToBeASubsetOfTapeAlphabetException()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_input_alphabet_is_no_subset_of_tape_alphabet.tmsim");
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

        [TestMethod]
        [ExpectedException(typeof(StateAlreadyExistsException))]
        public void AddState_throwsStateAlreadyExistsException() {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddState(ts);
            tm.AddState(ts);
        }

        [TestMethod]
        [ExpectedException(typeof(StateAlreadyExistsException))]
        public void EditState_throwsStateAlreadyExistsException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("id1");
            TuringState ts2 = new TuringState("id2");
            tm.AddState(ts1);
            tm.AddState(ts2);
            tm.EditState(ts1, ts2);
        }

        [TestMethod]
        [ExpectedException(typeof(StateDoesNotExistException))]
        public void RemoveState_throwsStateDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.RemoveState(ts);
        }

        [TestMethod]
        [ExpectedException(typeof(TransitionAlreadyExistsException))]
        public void AddTransition_throwsTransitionAlreadyExistsException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddState(ts);
            tm.AddSymbol('a', true);
            TuringTransition tt = new TuringTransition(ts, ts, new List<char>{'a'}, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left});
            tm.AddTransition(tt);
            tm.AddTransition(tt);
        }

        [TestMethod]
        [ExpectedException(typeof(SourceStateOfTransitionDoesNotExistException))]
        public void AddTransition_throwsSourceStateOfTransitionDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddSymbol('a', true);
            TuringTransition tt = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.AddTransition(tt);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetStateOfTransitionDoesNotExistException))]
        public void AddTransition_throwsTargetStateOfTransitionDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts1 = new TuringState("id1");
            TuringState ts2 = new TuringState("id2");
            tm.AddState(ts1);
            tm.AddSymbol('a', true);
            TuringTransition tt = new TuringTransition(ts1, ts2, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.AddTransition(tt);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadSymbolDoesNotExistException))]
        public void AddTransition_throwsReadSymbolDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddState(ts);
            TuringTransition tt = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.AddTransition(tt);
        }

        [TestMethod]
        [ExpectedException(typeof(WriteSymbolDoesNotExistException))]
        public void AddTransition_throwsWriteSymbolDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddSymbol('a', true);
            tm.AddState(ts);
            TuringTransition tt = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'b' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.AddTransition(tt);
        }

        [TestMethod]
        [ExpectedException(typeof(NumberOfTapesDoesNotMatchToTransitionDefinitionException))]
        public void AddTransition_throwsNumberOfTapesDoesNotMatchToTransitionDefinitionException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddSymbol('a', true);
            tm.AddSymbol('b', true);
            tm.AddState(ts);
            TuringTransition tt = new TuringTransition(ts, ts, new List<char> { 'a', 'a' }, new List<char> { 'b', 'b' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left, TuringTransition.Direction.Left });
            tm.AddTransition(tt);
        }


        [TestMethod]
        [ExpectedException(typeof(TransitionAlreadyExistsException))]
        public void EditTransition_throwsTransitionAlreadyExistsException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            tm.AddSymbol('a', true);
            tm.AddState(ts);
            TuringTransition tt1 = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            TuringTransition tt2 = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.AddTransition(tt1);
            tm.AddTransition(tt2);
            tm.EditTransition(tt1, tt2);
        }

        [TestMethod]
        [ExpectedException(typeof(TransitionDoesNotExistException))]
        public void EditTransition_throwsTransitionDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            TuringTransition tt = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.EditTransition(tt, tt);
        }


        [TestMethod]
        [ExpectedException(typeof(TransitionDoesNotExistException))]
        public void RemoveTransition_throwsTransitionDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            TuringState ts = new TuringState("id1");
            TuringTransition tt = new TuringTransition(ts, ts, new List<char> { 'a' }, new List<char> { 'a' }, new List<TuringTransition.Direction> { TuringTransition.Direction.Left });
            tm.RemoveTransition(tt);
        }

        [TestMethod]
        [ExpectedException(typeof(SymbolAlreadyExistsException))]
        public void AddSymbol_throwsSymbolAlreadyExistsException()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            tm.AddSymbol('a', false);
        }

        [TestMethod]
        [ExpectedException(typeof(SymbolCanNotBeInputAndBlankException))]
        public void AddSymbol_throwsSymbolCanNotBeInputAndBlankException()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(SymbolDoesNotExistException))]
        public void Editymbol_throwsSymbolDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            tm.EditSymbol('a', false);
        }

        [TestMethod]
        [ExpectedException(typeof(SymbolCanNotBeInputAndBlankException))]
        public void Editymbol_throwsSymbolCanNotBeInputAndBlankException()
        {
            TuringMachine tm = new TuringMachine();
            tm.AddSymbol('a', true);
            tm.EditSymbol('a', true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(SymbolDoesNotExistException))]
        public void RemoveSymbol_throwsSymbolDoesNotExistException()
        {
            TuringMachine tm = new TuringMachine();
            tm.RemoveSymbol('a');
        }

        [TestMethod]
        [ExpectedException(typeof(WordIsNoValidInputException))]
        public void WriteTapeWord_throwsWordIsNoValidInputException()
        {
            TuringMachine tm = new TuringMachine();
            tm.WriteTapeWord("Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(BlankCharMustBeSetException))]
        public void AdvanceState_throwsBlankCharMusBeSetException_forEmptyTuringMachine()
        {
            TuringMachine tm = new TuringMachine();
            Assert.IsFalse(tm.AdvanceState());
        }
    }
}
