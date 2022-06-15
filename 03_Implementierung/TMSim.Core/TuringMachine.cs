using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMSim.Core.Exceptions;
using static TMSim.Core.ImportExportStructure;

namespace TMSim.Core
{
    public class TuringMachine
    {
        private TuringAlphabet TapeAlphabet { get; set; }
        private TuringAlphabet InputAlphabet { get; set; }
        public List<char> TapeSymbols => TapeAlphabet.Symbols;
        public List<char> InputSymbols => InputAlphabet.Symbols;
        public char BlankChar { get; set; }
        public List<TuringState> States { get; private set; }
        public TuringState StartState { get; private set; }
        public List<TuringState> EndStates { get; private set; }
        public List<TuringTransition> Transitions { get; private set; }
        public TuringState CurrentState { get; private set; }
        public TuringTransition CurrentTransition { get; private set; }
        public List<TuringTape> Tapes { get; private set; }
        public TuringMachine()
        {
            TapeAlphabet = new TuringAlphabet("");
            InputAlphabet = new TuringAlphabet("");
            States = new List<TuringState>();
            BlankChar = '\0';
            StartState = null;
            CurrentState = null;
            EndStates = new List<TuringState>();
            Transitions = new List<TuringTransition>();
            Tapes = new List<TuringTape> { new TuringTape("", BlankChar) };
        }

        public void ImportFromTextFile(string filePath)
        {
            TuringMachine copy = GetCopy();
            string jsonString = System.IO.File.ReadAllText(filePath);
            try
            {
                FromJsonString(jsonString);
            }
            catch (Exception e)
            {
                TapeAlphabet = copy.TapeAlphabet;
                BlankChar = copy.BlankChar;
                InputAlphabet = copy.InputAlphabet;
                States = copy.States;
                StartState = copy.StartState;
                EndStates = copy.EndStates;
                Transitions = copy.Transitions;
                Tapes = copy.Tapes;
                Reset();
                if (e is InputAlphabetHasToBeASubsetOfTapeAlphabetException) throw e;
                else if (e is ReadSymbolDoesNotExistException) throw e;
                else if (e is SourceStateOfTransitionDoesNotExistException) throw e;
                else if (e is StateAlreadyExistsException) throw e;
                else if (e is SymbolAlreadyExistsException) throw e;
                else if (e is SymbolCanNotBeInputAndBlankException) throw e;
                else if (e is SymbolDoesNotExistException) throw e;
                else if (e is TargetStateOfTransitionDoesNotExistException) throw e;
                else if (e is TransitionAlreadyExistsException) throw e;
                else if (e is TransitionNumberOfTapesIsInconsistentException) throw e;
                else if (e is WriteSymbolDoesNotExistException) throw e;
                throw new ImportFileIsNotValidException();
            }
        }

        private void FromJsonString(string jsonString)
        {
            var tm = JsonConvert.DeserializeObject<ImportExportStructure>(jsonString);
            if (tm.States == null)
            {
                // States are not defined in inputfile
                tm.States = new List<State> { };
            }
            if (tm.EndStates == null)
            {
                // Endstates are not defined in inputfile
                tm.EndStates = new List<string> { };
            }
            if (tm.TapeAlphabet == null)
            {
                // TapeAlphabet is not defined in inputfile
                tm.TapeAlphabet = "";
            }
            if (tm.InputAlphabet == null)
            {
                // InputAlphabet is not defined in inputfile
                tm.InputAlphabet = "";
            }
            TapeAlphabet = new TuringAlphabet("");
            InputAlphabet = new TuringAlphabet("");
            foreach (char c in tm.TapeAlphabet.ToList())
            {
                AddSymbol(c, false);
            }
            foreach (char c in tm.InputAlphabet.ToList())
            {
                try
                {
                    EditSymbol(c, true);
                }
                catch (SymbolDoesNotExistException)
                {
                    throw new InputAlphabetHasToBeASubsetOfTapeAlphabetException();
                }
            }
            if (InputAlphabet.Symbols.Contains(tm.Blank))
            {
                throw new SymbolCanNotBeInputAndBlankException();
            }
            if (tm.Blank != '\0')
            {
                EditSymbol(tm.Blank, false, true);
            }

            States.Clear();
            EndStates.Clear();
            Transitions.Clear();
            Tapes.Clear();
            foreach (State state in tm.States)
            {
                bool isStart = false;
                bool isAccepting = false;
                if (state.Identifier == tm.StartState) isStart = true;
                if (tm.EndStates.Contains(state.Identifier)) isAccepting = true;
                AddState(new TuringState(state.Identifier, comment: state.Comment, isStart: isStart, isAccepting: isAccepting));
            }
            CurrentState = StartState;
            if (tm.Transitions.Count() > 0)
            {
                for (int i = 0; i < tm.Transitions[0].SymbolsRead.Count(); i++)
                {
                    Tapes.Add(new TuringTape("", BlankChar));
                }
            }
            else
            {
                // definition contains no transitions
                Tapes.Add(new TuringTape("", BlankChar));
            }
            foreach (Transition transition in tm.Transitions)
            {
                AddTransition(
                    new TuringTransition(
                        States.Find(item => item.Identifier == transition.SourceState),
                        States.Find(item => item.Identifier == transition.TargetState),
                        transition.SymbolsRead,
                        transition.SymbolsWrite,
                        transition.MoveDirections
                    ));
            }
        }

        public void ExportToTextFile(string filePath)
        {
            ImportExportStructure importExportStructure = new ImportExportStructure(this);
            string jsonString = JsonConvert.SerializeObject(importExportStructure, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonString);


        }

        public bool AdvanceState()
        {
            if (BlankChar == '\0')
            {
                throw new BlankCharMustBeSetException();
            }
            if (StartState == null)
            {
                throw new NoStartStateException();
            }
            try
            {
                CurrentTransition = GetTransition();
                for (int i = 0; i < Tapes.Count(); i++)
                {
                    Tapes[i].SetCurrentSymbol(CurrentTransition.SymbolsWrite[i]);
                    if (CurrentTransition.MoveDirections[i] == TuringTransition.Direction.Right) Tapes[i].MoveRight();
                    else if (CurrentTransition.MoveDirections[i] == TuringTransition.Direction.Left) Tapes[i].MoveLeft();
                }
                CurrentState = CurrentTransition.Target;
            }
            catch (TransitionNotFoundException)
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            List<TuringTape> newTapes = new List<TuringTape>();
            foreach (TuringTape _ in Tapes)
            {
                newTapes.Add(new TuringTape("", BlankChar));
            }
            Tapes = newTapes;
            CurrentState = StartState;
        }

        public bool CheckIsEndState()
        {
            if (CurrentState != null && EndStates.Contains(CurrentState))
            {
                return true;
            }
            return false;
        }

        private TuringTransition GetTransition()
        {
            if (CurrentState == null) CurrentState = StartState;
            foreach (TuringTransition transition in CurrentState.OutgoingTransitions)
            {
                if (transition.CheckIfTransitionShouldBeActive(Tapes, CurrentState)) return transition;
            }
            throw new TransitionNotFoundException();
        }

        public void AddState(TuringState ts)
        {
            foreach (TuringState state in States) if (state.Identifier == ts.Identifier) throw new StateAlreadyExistsException();
            States.Add(ts);
            if (ts.IsStart)
            {
                if (StartState != null) StartState.IsStart = false;
                StartState = ts;
            }

            if (ts.IsAccepting) { EndStates.Add(ts); }
            Reset();
        }

        public void EditState(TuringState tsOld, TuringState tsNew)
        {
            foreach (TuringState state in States)
            {
                if (state.Identifier == tsNew.Identifier && state.Identifier != tsOld.Identifier)
                    throw new StateAlreadyExistsException();
            }
            tsOld.Identifier = tsNew.Identifier;
            tsOld.Comment = tsNew.Comment;

            tsOld.IsAccepting = tsNew.IsAccepting;
            if (tsOld.IsAccepting && !EndStates.Contains(tsOld)) { EndStates.Add(tsOld); }
            if (!tsOld.IsAccepting && EndStates.Contains(tsOld)) { EndStates.Remove(tsOld); }

            if (tsOld.IsStart && !tsNew.IsStart)
            {
                StartState = null;
            }
            else if (!tsOld.IsStart && tsNew.IsStart)
            {
                if (StartState != null && StartState != tsOld) StartState.IsStart = false;
                StartState = tsOld;
            }
            tsOld.IsStart = tsNew.IsStart;
            Reset();
        }

        public void RemoveState(TuringState ts)
        {
            if (!States.Contains(ts)) throw new StateDoesNotExistException();
            foreach (TuringTransition tt in ts.OutgoingTransitions)
            {
                tt.Target.IncomingTransitions.Remove(tt);
                Transitions.Remove(tt);
            }
            foreach (TuringTransition tt in ts.IncomingTransitions)
            {
                tt.Source.OutgoingTransitions.Remove(tt);
                Transitions.Remove(tt);
            }
            if (EndStates.Contains(ts)) EndStates.Remove(ts);
            if (StartState == ts) StartState = null;
            States.Remove(ts);
            Reset();
        }

        public void AddTransition(TuringTransition tt)
        {
            if (checkTransitionAlreadyExists(tt)) throw new TransitionAlreadyExistsException();
            if (!States.Contains(tt.Source)) throw new SourceStateOfTransitionDoesNotExistException();
            if (!States.Contains(tt.Target)) throw new TargetStateOfTransitionDoesNotExistException();
            if (Tapes.Count() != tt.SymbolsRead.Count()) throw new NumberOfTapesDoesNotMatchToTransitionDefinitionException();
            foreach (char c in tt.SymbolsRead) if (!TapeAlphabet.Symbols.Contains(c)) throw new ReadSymbolDoesNotExistException(c.ToString());
            foreach (char c in tt.SymbolsWrite) if (!TapeAlphabet.Symbols.Contains(c)) throw new WriteSymbolDoesNotExistException(c.ToString());
            Transitions.Add(tt);
            tt.Source.OutgoingTransitions.Add(tt);
            tt.Target.IncomingTransitions.Add(tt);
            Reset();
        }

        public void EditTransition(TuringTransition ttOld, TuringTransition ttNew)
        {
            RemoveTransition(ttOld); // remove unused Symbols?
            AddTransition(ttNew);
        }

        public void RemoveTransition(TuringTransition tt, bool removeUnusedSymbols = false)
        {
            if (!Transitions.Contains(tt)) throw new TransitionDoesNotExistException();
            tt.Source.OutgoingTransitions.Remove(tt);
            tt.Target.IncomingTransitions.Remove(tt);
            Transitions.Remove(tt);
            if (removeUnusedSymbols)
            {
                foreach (char symbolToRemove in tt.SymbolsRead)
                {
                    bool shouldBeRemoved = true;
                    foreach (TuringTransition transition in Transitions)
                    {
                        if (tt.SymbolsRead.Contains(symbolToRemove) || tt.SymbolsWrite.Contains(symbolToRemove))
                        {
                            shouldBeRemoved = false;
                            break;
                        }
                    }
                    if (shouldBeRemoved)
                    {
                        RemoveSymbol(symbolToRemove);
                    }
                }
            }
            Reset();
        }

        public void AddSymbol(char c, bool isInInput, bool isBlank = false)
        {
            if (isInInput && isBlank) throw new SymbolCanNotBeInputAndBlankException();
            if (TapeAlphabet.Symbols.Contains(c)) throw new SymbolAlreadyExistsException();
            else
            {
                TapeAlphabet.Symbols.Add(c);
                if (isInInput) InputAlphabet.Symbols.Add(c);
                if (isBlank)
                {
                    BlankChar = c;
                }
            }
            Reset();
        }

        public void EditSymbol(char c, bool isInInput, bool isBlank = false)
        {
            if (!TapeAlphabet.Symbols.Contains(c)) throw new SymbolDoesNotExistException();
            if (isInInput && isBlank) throw new SymbolCanNotBeInputAndBlankException();
            if (!InputAlphabet.Symbols.Contains(c) && TapeAlphabet.Symbols.Contains(c) && isInInput) InputAlphabet.Symbols.Add(c);
            else if (InputAlphabet.Symbols.Contains(c) && !isInInput) InputAlphabet.Symbols.Remove(c);
            if (isBlank)
            {
                BlankChar = c;
            }
            if(isBlank == false && c == BlankChar)
            {
                BlankChar = '\0';
            }
            Reset();
        }

        public void RemoveSymbol(char c)
        {
            if (!TapeAlphabet.Symbols.Contains(c)) throw new SymbolDoesNotExistException();
            if (InputAlphabet.Symbols.Contains(c)) InputAlphabet.Symbols.Remove(c);
            if (TapeAlphabet.Symbols.Contains(c)) TapeAlphabet.Symbols.Remove(c);
            List<TuringTransition> TransitionsToRemove = new List<TuringTransition>();
            foreach (TuringTransition tt in Transitions)
            {
                if (tt.SymbolsRead.Contains(c) || tt.SymbolsWrite.Contains(c)) TransitionsToRemove.Add(tt);
            }
            foreach (TuringTransition tt in TransitionsToRemove)
            {
                RemoveTransition(tt);
            }
            if (c == BlankChar)
            {
                BlankChar = '\0';
            }
            Reset();
        }

        public TuringMachine GetCopy()
        {
            ImportExportStructure importExportStructure = new ImportExportStructure(this);
            string jsonString = JsonConvert.SerializeObject(importExportStructure, Formatting.Indented);
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.FromJsonString(jsonString);
            return turingMachine;
        }

        public void WriteTapeWord(string inputWord)
        {
            if (!InputAlphabet.WordIsContainedIn(inputWord) || inputWord.Length == 0) throw new WordIsNoValidInputException();
            //TODO: more than one tape?
            foreach (var tape in Tapes)
            {
                tape.Content = inputWord;
            }
        }
        private bool checkTransitionAlreadyExists(TuringTransition tt)
        {
            foreach (TuringTransition transition in Transitions)
            {
                if (transition.Source == tt.Source && transition.SymbolsRead.SequenceEqual(tt.SymbolsRead)) return true;
            }
            return false;
        }
    }
}