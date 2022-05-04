using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.model
{
    class TuringMaschine
    {
        private class TransitionNotFound : Exception
        {
            public TransitionNotFound(string message) : base(message)
            {
            }
        }

        public Alphabet BandAlphabet { get; set; }
        public char BlankChar { get; set; }
        public Alphabet InputAlphabet { get; set; }
        public List<TuringState> States { get; set; }
        public TuringState StartState { get; set; }
        public List<TuringState> EndStates { get; set; }
        public List<TuringTransition> Transitions { get; set; }

        public TuringState CurrentState { get; private set; }
        public List<TuringBand> Bands { get; set; }

        public TuringMaschine(Alphabet bandAlphabet, char blankChar, Alphabet inputAlphabet, 
            List<TuringState> states, TuringState startState, List<TuringState> endStates, 
            List<TuringTransition> transitions)
        {
            this.BandAlphabet = bandAlphabet;
            this.BlankChar = blankChar;
            this.InputAlphabet = inputAlphabet;
            this.States = states;
            this.StartState = startState;
            this.CurrentState = startState;
            this.EndStates = endStates;
            this.Transitions = transitions;
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
                Bands[0].SetCurrentSymbol(transition.SymbolWrite);
                if (transition.MoveDirection == TuringTransition.Direction.Right) Bands[0].MoveRight();
                else if (transition.MoveDirection == TuringTransition.Direction.Left) Bands[0].MoveLeft();
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
                if (transition.Source == CurrentState && transition.SymbolRead == Bands[0].GetCurrentSymbol()) return transition;
            }
            throw new TransitionNotFound("Can not find transition");
        }
    }
}
