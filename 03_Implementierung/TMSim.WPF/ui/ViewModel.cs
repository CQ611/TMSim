using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using TMSim.Core;

namespace TMSim.WPF
{
    public class ViewModel : ObservableObject
    {
        #region RelayCommands
        public RelayCommand StartPauseSimulation { get; set; }
        public RelayCommand StepSimulation { get; set; }
        public RelayCommand StopSimulation { get; set; }
        public RelayCommand WriteTapeWord { get; set; }
        public RelayCommand SetSimulationTimerInterval { get; set; }
        public RelayCommand TransformTuringMachine { get; set; }
        public RelayCommand ImportFromTextFile { get; set; }
        public RelayCommand ExportToTextFile { get; set; }
        public RelayCommand ClearTuringMachine { get; set; }
        public RelayCommand LoadExample { get; set; }
        public RelayCommand ExitApplication { get; set; }
        #endregion

        #region BindingProperties
        private Visibility _startVisibility = Visibility.Visible;
        public Visibility StartVisibility
        {
            get
            {
                return _startVisibility;
            }
            set
            {
                _startVisibility = value;
                OnPropertyChanged("StartVisibility");
            }
        }

        private Visibility _stopVisibility = Visibility.Hidden;
        public Visibility StopVisibility
        {
            get
            {
                return _stopVisibility;
            }
            set
            {
                _stopVisibility = value;
                OnPropertyChanged("StopVisibility");
            }
        }
        #endregion

        public ViewModel()
        {
            StartPauseSimulation = new RelayCommand((o) => { OnStartPauseSimulation(); });
            StepSimulation = new RelayCommand((o) => { OnStepSimulation(); });
            StopSimulation = new RelayCommand((o) => { OnStopSimulation(); });
            WriteTapeWord = new RelayCommand((o) => { OnWriteTapeWord(); });
            SetSimulationTimerInterval = new RelayCommand((o) => { OnSetSimulationTimerInterval(); });
            TransformTuringMachine = new RelayCommand((o) => { OnTansformTuringMachine(); });
            ImportFromTextFile = new RelayCommand((o) => { OnImportFromTextFile(); });
            ExportToTextFile = new RelayCommand((o) => { OnExportToTextFile(); });
            ClearTuringMachine = new RelayCommand((o) => { OnClearTuringMachine(); });
            LoadExample = new RelayCommand((o) => { OnLoadExample(); });
            ExitApplication = new RelayCommand((o) => { OnExitApplication(); });

            TM = new TuringMachine();

            TMModifier = new TuringMachineModifier(this, ref TM);
            DData = new DiagramData();
        }

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;
        public TuringMachineModifier TMModifier { get; }
        public DiagramData DData { get; private set; }

        private TuringMachine TM;

        public void OnTMChanged()
        {
            UpdateDiagramData();
            //UpdateTabledata();
        }

        private bool startIsActive = false;

        private void OnStartPauseSimulation()
        {
            if(startIsActive)
            {
                startIsActive = false;
                StartVisibility = Visibility.Visible;
                StopVisibility = Visibility.Hidden;
            }
            else
            {
                startIsActive = true;
                StartVisibility = Visibility.Hidden;
                StopVisibility = Visibility.Visible;
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

        public void OnTansformTuringMachine()
        {
            TM.TansformTuringMachine();
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

        public void OnClearTuringMachine()
        {
            TM = new TuringMachine(
                new Alphabet(""),
                ' ',
                new Alphabet(""),
                new List<TuringState>(),
                new TuringState(""),
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

        private void UpdateDiagramData()
        {
            //convert TM data to displayable data here
            //maybe move this to own diagrammconverter class
            var rand = new Random();

            DData.Nodes.Clear();
            DData.Connections.Clear();
            foreach(TuringState ts in TM.States)
            {
                Node n = new Node(ts, new Point(
                    rand.Next((int)DData.NodeSize / 2,
                        (int)(DData.Width - DData.NodeSize / 2)),
                    rand.Next((int)DData.NodeSize / 2,
                        (int)(DData.Height - DData.NodeSize / 2))),
                    TM.StartState == ts);
                DData.Nodes.Add(n.Identifier, n);
            }

            foreach(TuringTransition tt in TM.Transitions)
            {
                NodeConnection nc = new NodeConnection(
                    tt,
                    DData.Nodes[tt.Source.Identifier],
                    DData.Nodes[tt.Target.Identifier]
                    );
                DData.Connections.Add(nc);
            }

            OnPropertyChanged(nameof(DData));
        }

        public void OnExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
