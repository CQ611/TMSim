using System;
using System.Collections.Generic;
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
            SetSimulationTimerInterval = new RelayCommand((o) => { OnSetSimulationTimerInterval(); });
            TransformTuringMaschine = new RelayCommand((o) => { OnTansformTuringMaschine(); });
            ImportFromTextFile = new RelayCommand((o) => { OnImportFromTextFile(); });
            ExportToTextFile = new RelayCommand((o) => { OnExportToTextFile(); });
            ClearTuringMaschine = new RelayCommand((o) => { OnClearTuringMaschine(); });
        }

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;
        public RelayCommand StartPauseSimulation { get; set; }
        public RelayCommand StepSimulation { get; set; }
        public RelayCommand StopSimulation { get; set; }
        public RelayCommand WriteTapeWord { get; set; }
        public RelayCommand SetSimulationTimerInterval { get; set; }
        public RelayCommand TransformTuringMaschine { get; set; }
        public RelayCommand ImportFromTextFile { get; set; }
        public RelayCommand ExportToTextFile { get; set; }
        public RelayCommand ClearTuringMaschine { get; set; }


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

        public void OnSetSimulationTimerInterval()
        {
            throw new NotImplementedException("Hi");
        }

        public void OnTansformTuringMaschine()
        {
            TM.TansformTuringMaschine();
        }

        public void OnImportFromTextFile()
        {
            TM.ImportFromTextFile();
        }

        public void OnExportToTextFile()
        {
            TM.ExportToTextFile();
        }

        public void OnClearTuringMaschine()
        {
            TM = new TuringMaschine(
                new Alphabet(""),
                ' ',
                new Alphabet(""),
                new List<TuringState>(),
                new TuringState(),
                new List<TuringState>(),
                new List<TuringTransition>(),
                new List<TuringTape>()
                );
        }
    }
}
