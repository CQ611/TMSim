using System;
using TMSim.Core;

namespace TMSim.WPF
{
    class ViewModel : ObservableObject
    {
        public ViewModel()
        {
            StartPauseSimulation = new RelayCommand((o) => { OnStartPauseSimulation(); });
            StepSimulation = new RelayCommand((o) => { OnStepSimulation(); });
            StopSimulation = new RelayCommand((o) => { OnStopSimulation(); });
            WriteTapeWord = new RelayCommand((o) => { OnWriteTapeWord(); });
        }

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;
        public RelayCommand StartPauseSimulation { get; set; }
        public RelayCommand StepSimulation { get; set; }
        public RelayCommand StopSimulation { get; set; }
        public RelayCommand WriteTapeWord { get; set; }

        public TuringMaschine TM
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

        private void OnStartPauseSimulation()
        {
            throw new NotImplementedException("Hi");
        }

        public void OnStopSimulation()
        {
            throw new NotImplementedException("Hi");
        }

        public void OnStepSimulation()
        {
            throw new NotImplementedException("Hi");
        }

        public void OnWriteTapeWord()
        {
            throw new NotImplementedException("Hi");
        }

        public void SetSimulationTimerInterval()
        {
            throw new NotImplementedException("Hi");
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
