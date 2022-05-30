using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using TMSim.Core;
using TMSim.UI.PopupWindows;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        #region RelayCommands
        public RelayCommand StartPauseSimulation { get; set; }
        public RelayCommand StepSimulation { get; set; }
        public RelayCommand StopSimulation { get; set; }
        public RelayCommand WriteTapeWord { get; set; }
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
        public RelayCommand SingleViewSelected { get; set; }
        public RelayCommand MultiViewSelected { get; set; }
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

        private Visibility _singleViewVisibility = Visibility.Visible;
        public Visibility SingleViewVisibility
        {
            get
            {
                return _singleViewVisibility;
            }
            set
            {
                _singleViewVisibility = value;
                OnPropertyChanged("SingleViewVisibility");
            }
        }

        private Visibility _multiViewVisibility = Visibility.Hidden;
        public Visibility MultiViewVisibility
        {
            get
            {
                return _multiViewVisibility;
            }
            set
            {
                _multiViewVisibility = value;
                OnPropertyChanged("MultiViewVisibility");
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

        private bool _singleViewIsChecked = true;
        public bool SingleViewIsChecked
        {
            get
            {
                return _singleViewIsChecked;
            }
            set
            {
                _singleViewIsChecked = value;
                OnPropertyChanged("SingleViewIsChecked");
            }
        }

        private bool _multiViewIsChecked;
        public bool MultiViewIsChecked
        {
            get
            {
                return _multiViewIsChecked;
            }
            set
            {
                _multiViewIsChecked = value;
                OnPropertyChanged("MultiViewIsChecked");
            }
        }

        private string _tapeWordInput = String.Empty;
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

        #region Translation
        public string FileText { get => Translator.GetString("TEXT_File"); set { OnPropertyChanged(nameof(FileText)); } }
        public string OpenText { get => Translator.GetString("TEXT_Open"); set { OnPropertyChanged(nameof(OpenText)); } }
        public string SaveText { get => Translator.GetString("TEXT_Save"); set { OnPropertyChanged(nameof(SaveText)); } }
        public string NewText { get => Translator.GetString("TEXT_New"); set { OnPropertyChanged(nameof(NewText)); } }
        public string TransformText { get => Translator.GetString("TEXT_Transform"); set { OnPropertyChanged(nameof(TransformText)); } }
        public string Transformation1Text { get => Translator.GetString("TEXT_Transformation1"); set { OnPropertyChanged(nameof(Transformation1Text)); } }
        public string Transformation2Text { get => Translator.GetString("TEXT_Transformation2"); set { OnPropertyChanged(nameof(Transformation2Text)); } }
        public string Transformation3Text { get => Translator.GetString("TEXT_Transformation3"); set { OnPropertyChanged(nameof(Transformation3Text)); } }
        public string Transformation4Text { get => Translator.GetString("TEXT_Transformation4"); set { OnPropertyChanged(nameof(Transformation4Text)); } }
        public string Transformation5Text { get => Translator.GetString("TEXT_Transformation5"); set { OnPropertyChanged(nameof(Transformation5Text)); } }
        public string ExamplesText { get => Translator.GetString("TEXT_Examples"); set { OnPropertyChanged(nameof(ExamplesText)); } }
        public string ExitText { get => Translator.GetString("TEXT_Exit"); set { OnPropertyChanged(nameof(ExitText)); } }
        public string LanguageText { get => Translator.GetString("TEXT_Language"); set { OnPropertyChanged(nameof(LanguageText)); } }
        public string GermanText { get => Translator.GetString("TEXT_German"); set { OnPropertyChanged(nameof(GermanText)); } }
        public string EnglishText { get => Translator.GetString("TEXT_English"); set { OnPropertyChanged(nameof(EnglishText)); } }
        public string PreferencesText { get => Translator.GetString("TEXT_Preferences"); set { OnPropertyChanged(nameof(PreferencesText)); } }
        public string HighlightText { get => Translator.GetString("TEXT_Highlight"); set { OnPropertyChanged(nameof(HighlightText)); } }
        public string UploadText { get => Translator.GetString("TEXT_Upload"); set { OnPropertyChanged(nameof(UploadText)); } }
        public string InputwordText { get => Translator.GetString("TEXT_Inputword"); set { OnPropertyChanged(nameof(InputwordText)); } }
        public string SpeedLabelText { get => Translator.GetString("TEXT_SpeedLabel"); set { OnPropertyChanged(nameof(SpeedLabelText)); } }
        public string PopupIdentifierText { get => Translator.GetString("TEXT_PopupIdentifier"); set { OnPropertyChanged(nameof(PopupIdentifierText)); } }
        public string PopupStartText { get => Translator.GetString("TEXT_PopupStart"); set { OnPropertyChanged(nameof(PopupStartText)); } }
        public string PopupAcceptText { get => Translator.GetString("TEXT_PopupAccept"); set { OnPropertyChanged(nameof(PopupAcceptText)); } }
        public string PopupOKText { get => Translator.GetString("TEXT_PopupOK"); set { OnPropertyChanged(nameof(PopupOKText)); } }
        public string PopupCancelText { get => Translator.GetString("TEXT_PopupCancel"); set { OnPropertyChanged(nameof(PopupCancelText)); } }
        public string PopupIdentifierTextText { get => Translator.GetString("TEXT_PopupIdentifierText"); set { OnPropertyChanged(nameof(PopupIdentifierTextText)); } }
        public string PopupAddSymbolText { get => Translator.GetString("TEXT_PopupAddSymbol"); set { OnPropertyChanged(nameof(PopupAddSymbolText)); } }
        public string PopupSymbolText { get => Translator.GetString("TEXT_PopupSymbol"); set { OnPropertyChanged(nameof(PopupSymbolText)); } }
        public string PopupIsInputAlphabetText { get => Translator.GetString("TEXT_PopupIsInputAlphabet"); set { OnPropertyChanged(nameof(PopupIsInputAlphabetText)); } }
        public string PopupBlankText { get => Translator.GetString("TEXT_PopupBlank"); set { OnPropertyChanged(nameof(PopupBlankText)); } }
        public string ReadSymbolsText { get => Translator.GetString("TEXT_ReadSymbols"); set { OnPropertyChanged(nameof(ReadSymbolsText)); } }
        public string WriteSymbolsText { get => Translator.GetString("TEXT_WriteSymbols"); set { OnPropertyChanged(nameof(WriteSymbolsText)); } }
        public string SourceStateText { get => Translator.GetString("TEXT_SourceState"); set { OnPropertyChanged(nameof(SourceStateText)); } }
        public string TargetStateText { get => Translator.GetString("TEXT_TargetState"); set { OnPropertyChanged(nameof(TargetStateText)); } }
        public string CommentText { get => Translator.GetString("TEXT_Comment"); set { OnPropertyChanged(nameof(CommentText)); } }
        public string DirectionsText { get => Translator.GetString("TEXT_Directions"); set { OnPropertyChanged(nameof(DirectionsText)); } }


        private void RefreshTextFromUi()
        {
            FileText = Translator.GetString("TEXT_File");
            OpenText = Translator.GetString("TEXT_Open");
            SaveText = Translator.GetString("TEXT_Save");
            NewText = Translator.GetString("TEXT_New");
            ExamplesText = Translator.GetString("TEXT_Examples");
            ExitText = Translator.GetString("TEXT_Exit");
            TransformText = Translator.GetString("TEXT_Transform");
            Transformation1Text = Translator.GetString("TEXT_Transformation1");
            Transformation2Text = Translator.GetString("TEXT_Transformation2");
            Transformation3Text = Translator.GetString("TEXT_Transformation3");
            Transformation4Text = Translator.GetString("TEXT_Transformation4");
            Transformation5Text = Translator.GetString("TEXT_Transformation5");
            LanguageText = Translator.GetString("TEXT_Language");
            GermanText = Translator.GetString("TEXT_German");
            EnglishText = Translator.GetString("TEXT_English");
            PreferencesText = Translator.GetString("TEXT_Preferences");
            HighlightText = Translator.GetString("TEXT_Highlight");
            UploadText = Translator.GetString("TEXT_Upload");
            SpeedLabelText = Translator.GetString("TEXT_SpeedLabel");
            InputwordText = Translator.GetString("TEXT_Inputword");
            PopupIdentifierText = Translator.GetString("TEXT_PopupIdentifier");
            PopupStartText = Translator.GetString("TEXT_PopupStart");
            PopupAcceptText = Translator.GetString("TEXT_PopupAccept");
            PopupOKText = Translator.GetString("TEXT_PopupOK");
            PopupCancelText = Translator.GetString("TEXT_PopupCancel");
            PopupIdentifierTextText = Translator.GetString("TEXT_PopupIdentifierText");
        }
        #endregion

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;
        public DiagramData DData { get; set; }

        public static ResourceManager Translator;
        public TuringMachine TM;
        private DispatcherTimer timmy;

        public ViewModel()
        {
            StartPauseSimulation = new RelayCommand((o) => { OnStartPauseSimulation(); });
            StepSimulation = new RelayCommand((o) => { OnStepSimulation(); });
            StopSimulation = new RelayCommand((o) => { OnStopSimulation(); });
            WriteTapeWord = new RelayCommand((o) => { OnWriteTapeWord(); });
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
            SingleViewSelected = new RelayCommand((o) => { OnSingleViewSelected(); });
            MultiViewSelected = new RelayCommand((o) => { OnMultiViewSelected(); });

            TM = new TuringMachine();

            DData = new DiagramData();
            InitResoureManager();

            timmy = new DispatcherTimer();
            timmy.Tick += Timmy_Tick;
            SetTimerInterval();
        }

        private void InitResoureManager()
        {
            Translator = new ResourceManager("TMSim.UI.Resources.Strings", Assembly.GetExecutingAssembly());
            if (CultureInfo.CurrentCulture.Name == "de-DE")
            {
                GermanLanguageIsChecked = true;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                EnglishLanguageIsChecked = true;
            }
        }

        public void OnTMChanged()
        {
            UpdateDiagramData();
            UpdateTapeData();
            UpdateTableData();
        }

        private bool startIsActive = false;


        private void OnStartPauseSimulation()
        {
            if (startIsActive)
            {
                startIsActive = false;
                StartVisibility = Visibility.Visible;
                StopVisibility = Visibility.Hidden;
                timmy.Stop();
            }
            else
            {
                startIsActive = true;
                StartVisibility = Visibility.Hidden;
                StopVisibility = Visibility.Visible;
                timmy.Start();
            }
            OnTMChanged();
        }

        private void Timmy_Tick(object sender, EventArgs e)
        {
            OnStepSimulation();
        }

        private void SetTimerInterval()
        {
            timmy.Interval = TimeSpan.FromMilliseconds(TapeVelocity * 1.5);
        }

        private void OnStopSimulation()
        {
            TM.Reset();
            startIsActive = false;
            StartVisibility = Visibility.Visible;
            StopVisibility = Visibility.Hidden;
            timmy.Stop();
            OnTMChanged();
        }

        private void OnStepSimulation()
        {
            if (!TM.AdvanceState())
            {
                startIsActive = false;
                StartVisibility = Visibility.Visible;
                StopVisibility = Visibility.Hidden;
                timmy.Stop();
                CheckIfRunWasSuccessful();
            }
            OnTMChanged();
        }

        private void CheckIfRunWasSuccessful()
        {
            if (TM.CheckIsEndState())
            {
                // Todo: Übersetzen
                InfoMessage = "Endzustand erreicht";
                InfoMessageColor = GetBrushFromEnum(MessageColor.DarkGreen);
            }
            else
            {
                InfoMessage = "Endzustand nicht erreicht";
                InfoMessageColor = GetBrushFromEnum(MessageColor.Red);
            }
        }

        private void OnWriteTapeWord()
        {
            TM.WriteTapeWord(TapeWordInput);
            LoadTapeContent();
        }

        private void OnTansformTuringMachine()
        {
            //TM.TansformTuringMachine();
            throw new NotImplementedException("OnTransformTuringMachine >> ViewModel");
        }

        private void OnTransformation1()
        {
            InfoMessage = "OnTransformation1 >> ViewModel";
            InfoMessageColor = GetBrushFromEnum(MessageColor.Red);
            //throw new NotImplementedException("OnTransformation1 >> ViewModel");
        }

        private void OnTransformation2()
        {
            TransformT2();
        }

        private void OnTransformation3()
        {
            Transformation3Dialog t3d = new Transformation3Dialog();
            if (t3d.ShowDialog() == true)
            {
                char newBlank = t3d.Blank;
                TransformT3(newBlank);
            }
        }

        private void OnTransformation4()
        {
            throw new NotImplementedException("OnTransformation4 >> ViewModel");
        }

        private void OnTransformation5()
        {
            TransformT5();
        }

        public void TransformT2()
        {
            TM = new Transformation2().Execute(TM);
            OnTMChanged();
        }

        public void TransformT3(char newBlank)
        {
            throw new NotImplementedException("TransformT3 >> ViewModel");
        }

        public void TransformT5()
        {
            TM = new Transformation5().Execute(TM);
            OnTMChanged();
        }

        private void OnImportFromTextFile()
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
                DeleteTapeContent();
                OnTMChanged();
            }
        }

        private void OnExportToTextFile()
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

        private void OnClearTuringMachine()
        {
            TM = new TuringMachine();
            OnTMChanged();
        }

        private void OnLoadExample()
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
                DeleteTapeContent();
                OnTMChanged();
            }
        }

        private void UpdateDiagramData()
        {
            DData = UpdateDiagramData(DData, TM);
            //OnPropertyChanged(nameof(DData));
            DData.OnForcePropertyChanged();
        }

        private void OnExitApplication()
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

            Translator = new ResourceManager("TMSim.UI.Resources.Strings", Assembly.GetExecutingAssembly());

            RefreshTextFromUi();
        }

        private void OnGermanLanguageSelected()
        {
            GermanLanguageIsChecked = true;
            EnglishLanguageIsChecked = false;
            SelectedLanguageChanged("de-DE");
        }

        private void OnEnglishLanguageSelected()
        {
            GermanLanguageIsChecked = false;
            EnglishLanguageIsChecked = true;
            SelectedLanguageChanged("en-US");
        }

        public static void QuickWarning(string message)
        {
            MessageBox.Show(message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void AddState()
        {
            AddStateDialog asd = new AddStateDialog($"q{TM.States.Count}", TM.States.Count<1);
            if (asd.ShowDialog() == true)
            {
                string identifier = asd.Identfier;
                bool isStart = asd.IsStart;
                bool isAccepting = asd.IsAccepting;
                string comment = "DEBUG: TODO: get comment from popup";
                //TODO: get comment from popup

                List<string> existingStates = new List<string>();
                TM.States.ForEach(ts => existingStates.Add(ts.Identifier));
                if (existingStates.Contains(identifier))
                {
                    QuickWarning($"State with identifier {identifier} already exists!");
                    return;
                }

                TM.AddState(new TuringState(identifier, comment, isStart, isAccepting));
                OnTMChanged();
            }
        }

        public void AddSymbol()
        {
            AddSymbolDialog asd = new AddSymbolDialog();
            if(asd.ShowDialog() == true)
            {
                char symbol = asd.Symbol;
                bool isInputAlphabet = asd.IsInInput;

                if(TM.InputSymbols.Contains(symbol) || TM.TapeSymbols.Contains(symbol))
                {
                    QuickWarning($"Symbol {symbol} already exists!");
                    return;
                }

                TM.AddSymbol(symbol, isInputAlphabet);
                OnTMChanged();
            }
        }


        public void RemoveState(string ident)
        {
            TuringState ts = TM.States.First(x => x.Identifier == ident);
            TM.RemoveState(ts);
            OnTMChanged();
        }

        public void AddTransition(TuringState source = null, TuringState target = null)
        {
            AddTransitionDialog atd = new AddTransitionDialog(TM.States, source, target);
            if (atd.ShowDialog() == true)
            {
                TM.AddTransition(new TuringTransition(
                    atd.Source, atd.Target, atd.SymbolsRead,
                    atd.SymbolsWrite, atd.Directions, atd.Comment));

                //TODO: add checkboxes to decide whether new symbols should be in input alphabet

                OnTMChanged();
            }
        }

        private void OnSingleViewSelected()
        {
            MultiViewIsChecked = false;
            SingleViewIsChecked = true;
            MultiViewVisibility = Visibility.Hidden;
            SingleViewVisibility = Visibility.Visible;
        }

        private void OnMultiViewSelected()
        {
            SingleViewIsChecked = false;
            MultiViewIsChecked = true;
            SingleViewVisibility = Visibility.Hidden;
            MultiViewVisibility = Visibility.Visible;
        }
    }
}