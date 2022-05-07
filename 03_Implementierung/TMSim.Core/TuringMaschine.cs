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

        public void ImportFromTextFile()
        {
            //opens dialog here, so no path needed
        }

        public void ExportToTextFile()
        {
            //opens dialog here, so no path needed
        }

        public bool AdvanceState()
        {
            try{
                TuringTransition transition = GetTransition();
                for (int i = 0; i < Tapes.Count(); i++) {
                    Tapes[i].SetCurrentSymbol(transition.SymbolsWrite[i]);
                    if (transition.MoveDirections[i] == TuringTransition.Direction.Right) Tapes[i].MoveRight();
                    else if (transition.MoveDirections[i] == TuringTransition.Direction.Left) Tapes[i].MoveLeft();
                }
                CurrentState = transition.Target;
            }
            catch(TransitionNotFound){
                return false;
            }
            return true;
        }

        public bool CheckIsEndState(){
            if (EndStates.Contains(CurrentState)){
                return true;
            }
            return false;
        }

        private TuringTransition GetTransition(){
            foreach(TuringTransition transition in Transitions){
                if (transition.Source == CurrentState) {
                    bool flag = true;
                    for (int i = 0; i < transition.SymbolsRead.Count() && flag; i++) {
                        if (transition.SymbolsRead[i] != Tapes[i].GetCurrentSymbol()) flag = false;
                    }
                    if (flag) {
                        return transition;
                    }
                }
            }
            throw new TransitionNotFound("Can not find transition");
        }
    }
}
