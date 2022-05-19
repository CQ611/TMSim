using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TMSim.Core.ImportExportStructure;

namespace TMSim.Core
{
    public class TuringMachine
    {
        private class TransitionNotFound : Exception
        {
            public TransitionNotFound(string message) : base(message)
            {
            }
        }

        public Alphabet TapeAlphabet { get; set; }
        public char BlankChar { get; set; }
        public Alphabet InputAlphabet { get; set; }
        public List<TuringState> States { get; set; }
        public TuringState StartState { get; set; }
        public List<TuringState> EndStates { get; set; }
        public List<TuringTransition> Transitions { get; set; }

        public TuringState CurrentState { get; private set; }
        public List<TuringTape> Tapes { get; set; }
        public List<Transformation> Transformations { get; set; }
        public TuringMachine()
        {
            TapeAlphabet = new Alphabet("");
            BlankChar = ' ';
            InputAlphabet = new Alphabet("");
            States = new List<TuringState>();
            StartState = new TuringState("");
            CurrentState = StartState;
            EndStates = new List<TuringState>();
            Transitions = new List<TuringTransition>();
            Tapes = new List<TuringTape>();
            Transformations = new List<Transformation>
            {
                new Transformation1(),
                new Transformation2(),
                new Transformation3(),
                new Transformation4(),
                new Transformation5()
            };
        }

        public TuringMachine(Alphabet tapeAlphabet, char blankChar, Alphabet inputAlphabet,
            List<TuringState> states, TuringState startState, List<TuringState> endStates,
            List<TuringTransition> transitions, List<TuringTape> tapes)
        {
            TapeAlphabet = tapeAlphabet;
            BlankChar = blankChar;
            InputAlphabet = inputAlphabet;
            States = states;
            StartState = startState;
            CurrentState = startState;
            EndStates = endStates;
            Transitions = new List<TuringTransition>();
            foreach (TuringTransition transition in transitions) 
            {
                AddTransition(transition);
            }
            Tapes = tapes;
            Transformations = new List<Transformation>
            {
                new Transformation1(),
                new Transformation2(),
                new Transformation3(),
                new Transformation4(),
                new Transformation5()
            };
        }


        public void TansformTuringMachine()
        {
            
        }

        public void ImportFromTextFile(string filePath)
        {
            string jsonString = System.IO.File.ReadAllText(filePath);
            FromJsonString(jsonString);
        }

        private void FromJsonString(string jsonString) {
            var tm = JsonConvert.DeserializeObject<ImportExportStructure>(jsonString);

            TapeAlphabet = new Alphabet(tm.TapeAlphabet);
            BlankChar = tm.Blank;
            InputAlphabet = new Alphabet(tm.InputAlphabet);

            States.Clear();
            EndStates.Clear();
            Transitions.Clear();
            Tapes.Clear();

            foreach (State state in tm.States)
            {
                States.Add(new TuringState(state.Identifier, state.Comment));
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
                TuringTransition transition = GetTransition();
                for (int i = 0; i < Tapes.Count(); i++)
                {
                    Tapes[i].SetCurrentSymbol(transition.SymbolsWrite[i]);
                    if (transition.MoveDirections[i] == TuringTransition.Direction.Right) Tapes[i].MoveRight();
                    else if (transition.MoveDirections[i] == TuringTransition.Direction.Left) Tapes[i].MoveLeft();
                }
                CurrentState = transition.Target;
            }
            catch (TransitionNotFound)
            {
                return false;
            }
            return true;
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
            throw new TransitionNotFound("Can not find transition");
        }

        public void AddState(string identifier, bool isStart = false, bool isAccepting = false, string comment = "")
        {
            TuringState ts = new TuringState(identifier, comment);
            States.Add(ts);
            if (isStart) { StartState = ts; }
            if (isAccepting) { EndStates.Add(ts); }
        }

        public void AddState(TuringState turingState, bool isStart = false, bool isAccepting = false)
        {
            States.Add(turingState);
            if (isStart) { StartState = turingState; }
            if (isAccepting) { EndStates.Add(turingState); }
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
            TapeAlphabet = new Alphabet(inputWord);

            //TODO: Anpassung der Funktion zum schreiben auf mehreren Baender?
            foreach(var tape in Tapes)
            {
                tape.Content = inputWord;
            }
        }
    }
}