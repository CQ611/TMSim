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
            StartState = new TuringState("");
            CurrentState = StartState;
            EndStates = new List<TuringState>();
            Transitions = new List<TuringTransition>();
            Tapes = new List<TuringTape> { new TuringTape("", BlankChar) };
        }

        public void ImportFromTextFile(string filePath)
        {
            string jsonString = System.IO.File.ReadAllText(filePath);
            FromJsonString(jsonString);
        }

        private void FromJsonString(string jsonString)
        {
            var tm = JsonConvert.DeserializeObject<ImportExportStructure>(jsonString);

            TapeAlphabet = new TuringAlphabet(tm.TapeAlphabet);
            BlankChar = tm.Blank;
            InputAlphabet = new TuringAlphabet(tm.InputAlphabet);

            States.Clear();
            EndStates.Clear();
            Transitions.Clear();
            Tapes.Clear();

            foreach (State state in tm.States)
            {
                States.Add(new TuringState(state.Identifier, comment:state.Comment));
            }

            foreach (TuringState state in States)
            {
                if (state.Identifier == tm.StartState)
                {
                    StartState = state;
                }
            }

            CurrentState = StartState;

            foreach (string endState in tm.EndStates)
            {
                foreach (TuringState state in States)
                {
                    if (state.Identifier == endState)
                        EndStates.Add(state);
                }
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
            if (ts.IsStart) { StartState.IsStart = false; StartState = ts; }
            if (ts.IsAccepting) { EndStates.Add(ts); }
        }

        public void EditState(TuringState tsOld, TuringState tsNew)
        {
            throw new NotImplementedException("EditState");
        }

        public void RemoveState(TuringState ts)
        {
            //TODO: remove State from EndStates, and StartState
            // maybe reassign StartState.

            // also all Transitions referencing this state need to be deleted
            ts.OutgoingTransitions.ForEach(tt => Transitions.Remove(tt));
            ts.IncomingTransitions.ForEach(tt => Transitions.Remove(tt));
            // are other Node still going to have a reference to tt?

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
            throw new NotImplementedException("EditTransition");
        }

        public void RemoveTransition(TuringTransition tt, bool removeUnusedSymbols = false)
        {
            //remove unused chars from alphabets
            throw new NotImplementedException("RemoveTransition");
        }

        public void AddSymbol(char c, bool isInInput)
        {
            throw new NotImplementedException("AddCharacter");
        }

        public void EditSymbol(char c, bool isInInput)
        {
            throw new NotImplementedException("EditCharacter");
        }

        public void RemoveSymbol(char c)
        {
            throw new NotImplementedException("RemoveCharacter");
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
            TapeAlphabet = new TuringAlphabet(inputWord);

            //TODO: Anpassung der Funktion zum schreiben auf mehreren Baender?
            foreach(var tape in Tapes)
            {
                tape.Content = inputWord;
            }
        }
    }
}