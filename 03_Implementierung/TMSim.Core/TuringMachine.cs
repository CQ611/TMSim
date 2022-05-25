using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            BlankChar = ' ';
            InputAlphabet = new TuringAlphabet("");
            States = new List<TuringState>();
            StartState = null;
            CurrentState = null;
            EndStates = new List<TuringState>();
            Transitions = new List<TuringTransition>();
            Tapes = new List<TuringTape> { new TuringTape("", BlankChar)};
        }

        public void ImportFromTextFile(string filePath)
        {
            string jsonString = System.IO.File.ReadAllText(filePath);
            FromJsonString(jsonString);
        }

        private void FromJsonString(string jsonString)
        {
            var tm = JsonConvert.DeserializeObject<ImportExportStructure>(jsonString);

            // TODO: check if InputAlphabet is a subsequence of TapeAlphabet
            // TODO: write Tests after errorhandling
            TapeAlphabet = new TuringAlphabet(tm.TapeAlphabet);
            BlankChar = tm.Blank;
            InputAlphabet = new TuringAlphabet(tm.InputAlphabet);

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
            if (Transitions.Count > 0)
            {
                for (int i = 0; i < Transitions[0].SymbolsRead.Count; i++)
                {
                    Tapes.Add(new TuringTape("", BlankChar));
                }
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
            CurrentState = StartState;
            Tapes = new List<TuringTape> { new TuringTape("", BlankChar) };
        }

        public bool CheckIsEndState()
        {
            if (EndStates.Contains(CurrentState))
            {
                return true;
            }
            return false;
        }

        private TuringTransition GetTransition()
        {
            foreach (TuringTransition transition in CurrentState.OutgoingTransitions)
            {
                if (transition.CheckIfTransitionShouldBeActive(Tapes, CurrentState)) return transition;
            }
            throw new TransitionNotFoundException("Can not find transition");
        }

        public void AddState(TuringState ts)
        {            
            States.Add(ts);
            if (ts.IsStart) {
                if (StartState != null) StartState.IsStart = false; 
                StartState = ts; 
            }
          
            if (ts.IsAccepting) { EndStates.Add(ts); }
        }

        public void EditState(TuringState tsOld, TuringState tsNew)
        {
            RemoveState(tsOld);
            AddState(tsNew);
        }

        public void RemoveState(TuringState ts)
        {
            // also all Transitions referencing this state need to be deleted
            foreach (TuringTransition tt in ts.OutgoingTransitions) {
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
        }

        public void AddTransition(TuringTransition tt)
        {
            Transitions.Add(tt);
            tt.Source.OutgoingTransitions.Add(tt);
            tt.Target.IncomingTransitions.Add(tt);
        }

        public void EditTransition(TuringTransition ttOld, TuringTransition ttNew)
        {
            RemoveTransition(ttOld); // remove unused Symbols?
            AddTransition(ttNew);
        }

        public void RemoveTransition(TuringTransition tt, bool removeUnusedSymbols = false)
        {
            tt.Source.OutgoingTransitions.Remove(tt);
            tt.Target.IncomingTransitions.Remove(tt);
            Transitions.Remove(tt);
            if (removeUnusedSymbols) {
                foreach (char symbolToRemove in tt.SymbolsRead) {
                    bool shouldBeRemoved = true;
                    foreach (TuringTransition transition in Transitions) {
                        if (tt.SymbolsRead.Contains(symbolToRemove) || tt.SymbolsWrite.Contains(symbolToRemove)) { 
                            shouldBeRemoved = false;
                            break;
                        }
                    }
                    if (shouldBeRemoved) {
                        RemoveSymbol(symbolToRemove);
                    }
                }
            }
        }

        public void AddSymbol(char c, bool isInInput)
        {
            if (!TapeAlphabet.Symbols.Contains(c)) TapeAlphabet.Symbols.Add(c);
            if (!InputAlphabet.Symbols.Contains(c) && isInInput) InputAlphabet.Symbols.Add(c);
        }

        public void EditSymbol(char c, bool isInInput)
        {
            if (!InputAlphabet.Symbols.Contains(c) && TapeAlphabet.Symbols.Contains(c) && isInInput) InputAlphabet.Symbols.Add(c);
            else if (InputAlphabet.Symbols.Contains(c) && !isInInput) InputAlphabet.Symbols.Remove(c);
            else throw new NotImplementedException("Edit Symbol can not be called for a symbol which is not added to an alphabet");
        }

        public void RemoveSymbol(char c)
        {
            if (InputAlphabet.Symbols.Contains(c)) InputAlphabet.Symbols.Remove(c);
            if (TapeAlphabet.Symbols.Contains(c)) TapeAlphabet.Symbols.Remove(c);
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
            //TODO: Anpassung der Funktion zum schreiben auf mehreren Baender?
            foreach(var tape in Tapes)
            {
                tape.Content = inputWord;
            }
        }
    }
}