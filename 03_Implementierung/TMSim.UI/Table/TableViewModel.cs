using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMSim.Core;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        public delegate void ClearTable();
        public event ClearTable ClearTableEvent;

        public delegate void LoadTable(
            List<char> TapeSymbols,
            List<char> InputSymbols,
            List<TuringState> States,
            TuringState StartState,
            List<TuringState> EndStates,
            List<TuringTransition> Transitions,
            TuringState CurrentState,
            TuringTransition CurrentTransition);
        public event LoadTable LoadTableEvent;


        private void UpdateTableData()
        {
            ClearTableEvent?.Invoke();

            LoadTableEvent?.Invoke(
                TM.TapeSymbols, 
                TM.InputSymbols, 
                TM.States,
                TM.StartState,
                TM.EndStates,
                TM.Transitions,
                TM.CurrentState,
                TM.CurrentTransition);
        }

        //TODO: Direkte Verbindung zu TuringState elminieren;
        //TODO: Direkte Verbindung zu TuringTransition elminieren;

        public List<char> TapeSymbols;
        public List<char> InputSymbols;
        public List<TuringState> States;
        public TuringState StartState;
        public List<TuringState> EndStates;
        public List<TuringTransition> Transitions;
        public TuringState CurrentState;
        public TuringTransition CurrentTransition;

    }
}
