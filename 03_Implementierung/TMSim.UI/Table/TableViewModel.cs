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

        public delegate void LoadTable(TuringMachine TM);
        public event LoadTable LoadTableEvent;

        public delegate void RefreshTableHighlight(TuringMachine TM);
        public event RefreshTableHighlight RefreshTableHighlightEvent;

        private void UpdateTableData()
        {
            ClearTableEvent?.Invoke();

            LoadTableEvent?.Invoke(TM);

            RefreshTableHighlightEvent?.Invoke(TM);
        }

        private void RefreshTableData()
        {
            RefreshTableHighlightEvent?.Invoke(TM);
        }
    }
}

