using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using TMSim.Core;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        #region RelayCommands
        public RelayCommand StartPauseSimulation { get; set; }
        public RelayCommand StepSimulation { get; set; }
        public RelayCommand StopSimulation { get; set; }
        public RelayCommand WriteTapeWord { get; set; }
        public RelayCommand SetSimulationTimerInterval { get; set; }
        public RelayCommand TransformTuringMachine { get; set; }
        public RelayCommand Transformation1 { get; set; }
        public RelayCommand Transformation2 { get; set; }
        public RelayCommand Transformation3 { get; set; }
        public RelayCommand Transformation4 { get; set; }
        public RelayCommand Transformation5 { get; set; }
        public RelayCommand ImportFromTextFile { get; set; }
        public RelayCommand ExportToTextFile { get; set; }
        public RelayCommand ClearTuringMachine { get; set; }
        public RelayCommand LoadExample { get; set; }
        public RelayCommand ExitApplication { get; set; }
        public RelayCommand GermanLanguageSelected { get; set; }
        public RelayCommand EnglishLanguageSelected { get; set; }
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

        private bool _germanLanguageIsChecked;
        public bool GermanLanguageIsChecked
        {
            get
            {
                return _germanLanguageIsChecked;
            }
            set
            {
                _germanLanguageIsChecked = value;
                OnPropertyChanged("GermanLanguageIsChecked");
            }
        }

        private bool _englishLanguageIsChecked;
        public bool EnglishLanguageIsChecked
        {
            get
            {
                return _englishLanguageIsChecked;
            }
            set
            {
                _englishLanguageIsChecked = value;
                OnPropertyChanged("EnglishLanguageIsChecked");
            }
        }

        private string _tapeWordInput;
        public string TapeWordInput
        {
            get
            {
                return _tapeWordInput;
            }
            set
            {
                _tapeWordInput = value;
                OnPropertyChanged("TapeWordInput");
            }
        }
        #endregion

        #region BindedTextPropertys
        public string FileText { get => resourceManager.GetString("TEXT_File"); set { OnPropertyChanged(nameof(FileText)); } }
        public string OpenText { get => resourceManager.GetString("TEXT_Open"); set { OnPropertyChanged(nameof(OpenText)); } }
        public string SaveText { get => resourceManager.GetString("TEXT_Save"); set { OnPropertyChanged(nameof(SaveText)); } }
        public string NewText { get => resourceManager.GetString("TEXT_New"); set { OnPropertyChanged(nameof(NewText)); } }
        public string Transformation1Text { get => resourceManager.GetString("TEXT_Transformation1"); set { OnPropertyChanged(nameof(Transformation1Text)); } }
        public string Transformation2Text { get => resourceManager.GetString("TEXT_Transformation2"); set { OnPropertyChanged(nameof(Transformation2Text)); } }
        public string Transformation3Text { get => resourceManager.GetString("TEXT_Transformation3"); set { OnPropertyChanged(nameof(Transformation3Text)); } }
        public string Transformation4Text { get => resourceManager.GetString("TEXT_Transformation4"); set { OnPropertyChanged(nameof(Transformation4Text)); } }
        public string Transformation5Text { get => resourceManager.GetString("TEXT_Transformation5"); set { OnPropertyChanged(nameof(Transformation5Text)); } }
        public string ExamplesText { get => resourceManager.GetString("TEXT_Examples"); set { OnPropertyChanged(nameof(ExamplesText)); } }
        public string ExitText { get => resourceManager.GetString("TEXT_Exit"); set { OnPropertyChanged(nameof(ExitText)); } }  
        #endregion

        private ResourceManager resourceManager;

        public ViewModel()
        {
            StartPauseSimulation = new RelayCommand((o) => { OnStartPauseSimulation(); });
            StepSimulation = new RelayCommand((o) => { OnStepSimulation(); });
            StopSimulation = new RelayCommand((o) => { OnStopSimulation(); });
            WriteTapeWord = new RelayCommand((o) => { OnWriteTapeWord(); });
            SetSimulationTimerInterval = new RelayCommand((o) => { OnSetSimulationTimerInterval(); });
            TransformTuringMachine = new RelayCommand((o) => { OnTansformTuringMachine(); });
            Transformation1 = new RelayCommand((o) => { OnTransformation1(); });
            Transformation2 = new RelayCommand((o) => { OnTransformation2(); });
            Transformation3 = new RelayCommand((o) => { OnTransformation3(); });
            Transformation4 = new RelayCommand((o) => { OnTransformation4(); });
            Transformation5 = new RelayCommand((o) => { OnTransformation5(); });
            ImportFromTextFile = new RelayCommand((o) => { OnImportFromTextFile(); });
            ExportToTextFile = new RelayCommand((o) => { OnExportToTextFile(); });
            ClearTuringMachine = new RelayCommand((o) => { OnClearTuringMachine(); });
            LoadExample = new RelayCommand((o) => { OnLoadExample(); });
            ExitApplication = new RelayCommand((o) => { OnExitApplication(); });
            GermanLanguageSelected = new RelayCommand((o) => { OnGermanLanguageSelected(); });
            EnglishLanguageSelected = new RelayCommand((o) => { OnEnglishLanguageSelected(); });
            RightButton = new RelayCommand((o) => { OnRightButton(); });
            LeftButton = new RelayCommand((o) => { OnLeftButton(); });

            TM = new TuringMachine();

            TMModifier = new TuringMachineModifier(this, ref TM);
            DData = new DiagramData();
            InitResoureManager();
        }

        private void InitResoureManager()
        {
            resourceManager = new ResourceManager("TMSim.UI.Resources.Strings", Assembly.GetExecutingAssembly());
            if(CultureInfo.CurrentCulture.Name == "de-DE")
            {
                GermanLanguageIsChecked = true;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-US") 
            {
                EnglishLanguageIsChecked = true;
            }
        }

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;
        public TuringMachineModifier TMModifier { get; }
        public DiagramData DData { get; private set; }

        public TuringMachine TM;

        public void OnTMChanged()
        {
            UpdateDiagramData();
            //UpdateTabledata();
        }

        private bool startIsActive = false;

        private void OnStartPauseSimulation()
        {
            if (startIsActive)
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
            TM.WriteTapeWord(TapeWordInput);
        }

        public void OnSetSimulationTimerInterval()
        {
            throw new NotImplementedException("Hi");
        }

        public void OnTansformTuringMachine()
        {
            //TM.TansformTuringMachine();
            throw new NotImplementedException("OnTransformTuringMachine >> ViewModel");
        }

        public void OnTransformation1()
        {
            throw new NotImplementedException("OnTransformation1 >> ViewModel");
        }

        public void OnTransformation2()
        {
            throw new NotImplementedException("OnTransformation2 >> ViewModel");
        }

        public void OnTransformation3()
        {
            throw new NotImplementedException("OnTransformation3 >> ViewModel");
        }

        public void OnTransformation4()
        {
            throw new NotImplementedException("OnTransformation4 >> ViewModel");
        }
        public void OnTransformation5()
        {
            TMModifier.TransformT5();
        }

        public void OnImportFromTextFile()
        {
            OpenFileDialog importFileDialog = new OpenFileDialog
            {
                Title = "Import turingmachine",
                Filter = "TMSim file (*.tmsim)|*.tmsim", 
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
                Filter = "TMSim file (*.tmsim)|*.tmsim",
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
            TM = new TuringMachine();
        }

        public void OnLoadExample()
        {
            OpenFileDialog loadExampleFileDialog = new OpenFileDialog
            {
                Title = "Load example turingmachine",
                Filter = "TMSim file (*.tmsim)|*.tmsim",
                FilterIndex = 2,
                InitialDirectory = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Examples")
            };
            if (loadExampleFileDialog.ShowDialog() == true)
            {
                TM.ImportFromTextFile(loadExampleFileDialog.FileName);
            }
        }

        private void UpdateDiagramData()
        {
            DData = UpdateDiagramData(DData, TM);
            OnPropertyChanged(nameof(DData));
        }

        public void OnExitApplication()
        {
            Application.Current.Shutdown();
        }

        public void SelectedLanguageChanged(string language)
        {
            var vCulture = new CultureInfo(language);

            Thread.CurrentThread.CurrentCulture = vCulture;
            Thread.CurrentThread.CurrentUICulture = vCulture;
            CultureInfo.DefaultThreadCurrentCulture = vCulture;
            CultureInfo.DefaultThreadCurrentUICulture = vCulture;

            try
            {
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("SelectedLanguageChanged Exeption: " + ex.ToString());
            }

            resourceManager = new ResourceManager("TMSim.UI.Resources.Strings", Assembly.GetExecutingAssembly());

            RefreshTextFromUi();
        }

        public void OnGermanLanguageSelected()
        {
            GermanLanguageIsChecked = true;
            EnglishLanguageIsChecked = false;
            SelectedLanguageChanged("de-DE");
        }

        public void OnEnglishLanguageSelected()
        {
            GermanLanguageIsChecked = false;
            EnglishLanguageIsChecked = true;
            SelectedLanguageChanged("en-US");
        }

        private void RefreshTextFromUi()
        {
            FileText = resourceManager.GetString("TEXT_File");
            OpenText = resourceManager.GetString("TEXT_Open");
            SaveText = resourceManager.GetString("TEXT_Save");
            NewText = resourceManager.GetString("TEXT_New");
            ExamplesText = resourceManager.GetString("TEXT_Examples");
            ExitText = resourceManager.GetString("TEXT_Exit");
            Transformation1Text = resourceManager.GetString("TEXT_Transformation1");
            Transformation2Text = resourceManager.GetString("TEXT_Transformation2");
            Transformation3Text = resourceManager.GetString("TEXT_Transformation3");
            Transformation4Text = resourceManager.GetString("TEXT_Transformation4");
            Transformation5Text = resourceManager.GetString("TEXT_Transformation5");
        }
    }
}