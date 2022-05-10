using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using TMSim.Core;

namespace TMSim.WPF
{
    class ViewModel : ObservableObject
    {
        #region RelayCommands
        public RelayCommand StartPauseSimulation { get; set; }
        public RelayCommand StepSimulation { get; set; }
        public RelayCommand StopSimulation { get; set; }
        public RelayCommand WriteTapeWord { get; set; }
        public RelayCommand SetSimulationTimerInterval { get; set; }
        public RelayCommand TransformTuringMaschine { get; set; }
        public RelayCommand ImportFromTextFile { get; set; }
        public RelayCommand ExportToTextFile { get; set; }
        public RelayCommand ClearTuringMaschine { get; set; }
        public RelayCommand LoadExample { get; set; }
        public RelayCommand ExitApplication { get; set; }
        #endregion

        #region BindingProperties
        //Unicode characters: https://en.wikipedia.org/wiki/List_of_Unicode_characters

        private string _switchLeftContent = char.ConvertFromUtf32(0x226A);
        public string SwitchLeftContent
        {
            get { return _switchLeftContent; }
        }

        private string _switchRightContent = char.ConvertFromUtf32(0x226B);
        public string SwitchRightContent
        {
            get { return _switchRightContent; }
        }

        private string startContent = char.ConvertFromUtf32(0x23F5);
        private string pauseContent = char.ConvertFromUtf32(0x23F8);
        private string _startPauseButtonContent = char.ConvertFromUtf32(0x23F5); 
        public string StartPauseButtonContent
        {
            get { return _startPauseButtonContent; }
            set
            {
                _startPauseButtonContent = value;
                OnPropertyChanged("StartPauseButtonContent");
            }
        }

        private string _stopButtonContent = char.ConvertFromUtf32(0x23F9);
        public string StopButtonContent
        {
            get { return _stopButtonContent; }
        }

        private string _stepButtonContent = char.ConvertFromUtf32(0x23EF);
        public string StepButtonContent
        {
            get { return _stepButtonContent; }
        }


        #endregion

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
            LoadExample = new RelayCommand((o) => { OnLoadExample(); });
            ExitApplication = new RelayCommand((o) => { OnExitApplication(); });
        }

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;



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

        private bool startIsActive = false;

        private void OnStartPauseSimulation()
        {
            if(startIsActive)
            {
                startIsActive = false;
                StartPauseButtonContent = startContent;
            }
            else
            {
                startIsActive = true;
                StartPauseButtonContent = pauseContent;
            }
            //todo
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
            OpenFileDialog importFileDialog = new OpenFileDialog
            {
                Title = "Import turingmachine",
                Filter = "Text file (*.txt)|*.txt",  //muss noch auf das entsprechende Dateiformat angepasst werden
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (importFileDialog.ShowDialog() == true)
            {
                TM.ImportFromTextFile(importFileDialog.FileName);
            }
        }

        public void OnExportToTextFile()
        {
            SaveFileDialog exportFileDialog = new SaveFileDialog
            {
                Title = "Export turingmachine",
                Filter = "Text file (*.txt)|*.txt",  //muss noch auf das entsprechende Dateiformat angepasst werden
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (exportFileDialog.ShowDialog() == true)
            {
                TM.ExportToTextFile(exportFileDialog.FileName);
            }
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

        public void OnLoadExample()
        {
            OpenFileDialog loadExampleFileDialog = new OpenFileDialog
            {
                Title = "Load example turingmachine",
                Filter = "Text file (*.txt)|*.txt",  //muss noch auf das entsprechende Dateiformat angepasst werden
                FilterIndex = 2,
                InitialDirectory = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)/*, "Examples"*/)  //Pfad muss angepasst werden
            };
            if (loadExampleFileDialog.ShowDialog() == true)
            {
                TM.ImportFromTextFile(loadExampleFileDialog.FileName); 
            }
        }

        public void OnExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
