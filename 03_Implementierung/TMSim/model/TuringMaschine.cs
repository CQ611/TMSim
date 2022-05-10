using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.model
{
    class TuringMaschine
    {
        public Alphabet BandAlphabet { get; set; }
        public char BlankChar { get; set; }
        public Alphabet InputAlphabet { get; set; }
        public List<TuringState> States { get; set; }
        public TuringState StartState { get; set; }
        public List<TuringState> EndStates { get; set; }
        public List<TuringTransition> Transitions { get; set; }

        public TuringState CurrentState { get; private set; }
        public List<TuringBand> Bands { get; set; }

        public TuringMaschine(Alphabet BandAlphabet, char BlankChar, Alphabet InputAlphabet, 
            List<TuringState> States, TuringState StartState, List<TuringState> EndStates, 
            List<TuringTransition> Transitions)
        {
            this.BandAlphabet = BandAlphabet;
            this.BlankChar = BlankChar;
            this.InputAlphabet = InputAlphabet;
            this.States = States;
            this.StartState = StartState;
            this.EndStates = EndStates;
            this.Transitions = Transitions;
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
            throw new NotImplementedException("Ajo");
            // true while running
            // updates CurrentState
        }
    }
}
