using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.ui
{
    class ViewModel
    {
        public ViewModel()
        {

        }
        public bool HighlightCurrentState { get; set; } = true;

        public model.TuringMaschine TM
        {
            get
            {
                // get when populating diagram and table
                throw new NotImplementedException("Hello there");
            }
            set
            {
                // set when accept button is pressed
                throw new NotImplementedException("Hello there");
            }
        }

        public void WriteBandWord()
        {

        }

        public void StartSimulation()
        {

        }

        public void PauseSimulation()
        {

        }

        public void StopSimulation()
        {

        }

        public void StepSimulation()
        {

        }

        public void SetSimulationTimerInterval()
        {

        }

        public void TansformTuringMaschine()
        {
            TM.TansformTuringMaschine();
        }

        public void ImportFromTextFile()
        {
            TM.ImportFromTextFile();
        }

        public void ExportToTextFile()
        {
            TM.ExportToTextFile();
        }
    }
}
