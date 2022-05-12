using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.Core
{
    public class TuringMaschine
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

        public TuringMaschine()
        {
            this.TapeAlphabet = new Alphabet("");
            this.BlankChar = ' ';
            this.InputAlphabet = new Alphabet("");
            this.States = new List<TuringState>();
            this.StartState = new TuringState("");
            this.CurrentState = this.StartState;
            this.EndStates = new List<TuringState>();
            this.Transitions = new List<TuringTransition>();
            this.Tapes = new List<TuringTape>();
        }

        public TuringMaschine(Alphabet tapeAlphabet, char blankChar, Alphabet inputAlphabet,
            List<TuringState> states, TuringState startState, List<TuringState> endStates,
            List<TuringTransition> transitions, List<TuringTape> tapes)
        {
            this.TapeAlphabet = tapeAlphabet;
            this.BlankChar = blankChar;
            this.InputAlphabet = inputAlphabet;
            this.States = states;
            this.StartState = startState;
            this.CurrentState = startState;
            this.EndStates = endStates;
            this.Transitions = transitions;
            this.Tapes = tapes;
        }


        public void TansformTuringMaschine()
        {

        }

        public void ImportFromTextFile(string filePath)
        {
            string jsonString = System.IO.File.ReadAllText(filePath);
            var tm = JsonConvert.DeserializeObject<ImportExportStructure>(jsonString);

            this.TapeAlphabet = new Alphabet(tm.TapeAlphabet);
            this.BlankChar = tm.Blank;
            this.InputAlphabet = new Alphabet(tm.InputAlphabet);

            this.States.Clear();
            this.EndStates.Clear();
            this.Transitions.Clear();

            for (int i = 0; i < tm.States.Count; i++)
            {
                this.States.Add(new TuringState(tm.States[i]));
            }

            foreach (TuringState state in this.States)
            {
                if (state.Identifier == tm.StartState)
                {
                    this.StartState = state;
                }
            }

            this.CurrentState = this.StartState;

            for (int i = 0; i < tm.EndStates.Count; i++)
            {
                foreach (TuringState state in this.States)
                {
                    if (state.Identifier == tm.EndStates[i])
                    {
                        this.EndStates.Add(state);
                    }
                }
            }

            for (int i = 0; i < tm.Transitions.Count; i++)
            {
                this.Transitions.Add(
                    new TuringTransition(this.States.Find(item => item.Identifier == tm.Transitions[i].SourceState),
                                        this.States.Find(item => item.Identifier == tm.Transitions[i].TargetState),
                                        tm.Transitions[i].SymbolsRead,
                                        tm.Transitions[i].SymbolsWrite,
                                        tm.Transitions[i].MoveDirections));
            }
        }

        public void ExportToTextFile(string filePath)
        {
            //filePath from FileDialog -> full path with filename
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
            foreach (TuringTransition transition in Transitions)
            {
                if (transition.Source == CurrentState)
                {
                    bool flag = true;
                    for (int i = 0; i < transition.SymbolsRead.Count() && flag; i++)
                    {
                        if (transition.SymbolsRead[i] != Tapes[i].GetCurrentSymbol()) flag = false;
                    }
                    if (flag)
                    {
                        return transition;
                    }
                }
            }
            throw new TransitionNotFound("Can not find transition");
        }
    }
}
