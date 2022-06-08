using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using TMSim.Core.Exceptions;
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

        private bool _diagramViewIsChecked = true;
        public bool DiagramViewIsChecked
        {
            get
            {
                return _diagramViewIsChecked;
            }
            set
            {
                _diagramViewIsChecked = value;
                if (_diagramViewIsChecked == false) TableViewIsChecked = true;
                OnPropertyChanged(nameof(DiagramViewIsChecked));
            }
        }

        private bool _tableViewIsChecked = true;
        public bool TableViewIsChecked
        {
            get
            {
                return _tableViewIsChecked;
            }
            set
            {
                _tableViewIsChecked = value;
                if (_tableViewIsChecked == false) DiagramViewIsChecked = true;
                OnPropertyChanged(nameof(TableViewIsChecked));
            }
        }

        private bool _highlightIsChecked = true;
        public bool HighlightIsChecked
        {
            get
            {
                return _highlightIsChecked;
            }
            set
            {
                _highlightIsChecked = value;
                OnPropertyChanged(nameof(HighlightIsChecked));
            }
        }

        private bool _animateDiagram = false;
        public bool AnimateDiagram
        {
            get
            {
                return _animateDiagram;
            }
            set
            {
                _animateDiagram = value;
                OnPropertyChanged(nameof(AnimateDiagram));
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
        public string PlayTooltip { get => Translator.GetString("TOOLTIP_Play"); set { OnPropertyChanged(nameof(PlayTooltip)); } }
        public string PauseTooltip { get => Translator.GetString("TOOLTIP_Pause"); set { OnPropertyChanged(nameof(PauseTooltip)); } }
        public string StopTooltip { get => Translator.GetString("TOOLTIP_Stop"); set { OnPropertyChanged(nameof(StopTooltip)); } }
        public string StepTooltip { get => Translator.GetString("TOOLTIP_Step"); set { OnPropertyChanged(nameof(StepTooltip)); } }
        public string SpeedTooltip { get => Translator.GetString("TOOLTIP_Speed"); set { OnPropertyChanged(nameof(SpeedTooltip)); } }
        public string EingabeTooltip { get => Translator.GetString("TOOLTIP_Eingabe"); set { OnPropertyChanged(nameof(EingabeTooltip)); } }
        public string UploadTooltip { get => Translator.GetString("TOOLTIP_Upload"); set { OnPropertyChanged(nameof(UploadTooltip)); } }
        public string Transformation1Tooltip { get => Translator.GetString("TOOLTIP_Transformation1"); set { OnPropertyChanged(nameof(Transformation1Tooltip)); } }
        public string Transformation2Tooltip { get => Translator.GetString("TOOLTIP_Transformation2"); set { OnPropertyChanged(nameof(Transformation2Tooltip)); } }
        public string Transformation3Tooltip { get => Translator.GetString("TOOLTIP_Transformation3"); set { OnPropertyChanged(nameof(Transformation3Tooltip)); } }
        public string Transformation4Tooltip { get => Translator.GetString("TOOLTIP_Transformation4"); set { OnPropertyChanged(nameof(Transformation4Tooltip)); } }
        public string Transformation5Tooltip { get => Translator.GetString("TOOLTIP_Transformation5"); set { OnPropertyChanged(nameof(Transformation5Tooltip)); } }
        public string ViewText { get => Translator.GetString("TEXT_View"); set { OnPropertyChanged(nameof(ViewText)); } }
        public string DiagramViewText { get => Translator.GetString("TEXT_DiagramView"); set { OnPropertyChanged(nameof(DiagramViewText)); } }
        public string TableViewText { get => Translator.GetString("TEXT_TableView"); set { OnPropertyChanged(nameof(TableViewText)); } }
        public string InvalidInputWordText { get => Translator.GetString("TEXT_InvalidInputWord"); set { OnPropertyChanged(nameof(InvalidInputWordText)); } }
        public string InvalidTapeNumberInDefinitionText { get => Translator.GetString("TEXT_InvalidTapeNumberInDefinition"); set { OnPropertyChanged(nameof(InvalidTapeNumberInDefinitionText)); } }
        public string ReadSymbolDoesNotExistText { get => Translator.GetString("TEXT_ReadSymbolDoesNotExist"); set { OnPropertyChanged(nameof(ReadSymbolDoesNotExistText)); } }
        public string SourceStateDoesNotExistText { get => Translator.GetString("TEXT_SourceStateDoesNotExist"); set { OnPropertyChanged(nameof(SourceStateDoesNotExistText)); } }
        public string StateDoesNotExistText { get => Translator.GetString("TEXT_StateDoesNotExist"); set { OnPropertyChanged(nameof(StateDoesNotExistText)); } }
        public string StateExistsText { get => Translator.GetString("TEXT_StateExists"); set { OnPropertyChanged(nameof(StateExistsText)); } }
        public string SymbolDoesNotExistText { get => Translator.GetString("TEXT_SymbolDoesNotExist"); set { OnPropertyChanged(nameof(SymbolDoesNotExistText)); } }
        public string SymbolExistsText { get => Translator.GetString("TEXT_SymbolExists"); set { OnPropertyChanged(nameof(SymbolExistsText)); } }
        public string TargetStateDoesNotExistText { get => Translator.GetString("TEXT_TargetStateDoesNotExist"); set { OnPropertyChanged(nameof(TargetStateDoesNotExistText)); } }
        public string TransitionDoesNotExistText { get => Translator.GetString("TEXT_TransitionDoesNotExist"); set { OnPropertyChanged(nameof(TransitionDoesNotExistText)); } }
        public string TransitionExistsText { get => Translator.GetString("TEXT_TransitionExists"); set { OnPropertyChanged(nameof(TransitionExistsText)); } }
        public string WriteSymbolDoesNotExistText { get => Translator.GetString("TEXT_WriteSymbolDoesNotExist"); set { OnPropertyChanged(nameof(WriteSymbolDoesNotExistText)); } }
        public string SimulationSuccessText { get => Translator.GetString("TEXT_Info_SimulationSuccess"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(SimulationSuccessText)); } }
        public string SimulationFailureText { get => Translator.GetString("TEXT_Info_SimulationFailure"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(SimulationFailureText)); } }
        public string SimulationIsRunningText { get => Translator.GetString("TEXT_Info_SimulationIsRunning"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(SimulationIsRunningText)); } }
        public string SimulationIsPausedText { get => Translator.GetString("TEXT_Info_SimulationIsPaused"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(SimulationIsPausedText)); } }
        public string SimulationIsStoppedText { get => Translator.GetString("TEXT_Info_SimulationIsStopped"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(SimulationIsStoppedText)); } }
        public string SimulationSingleStepText { get => Translator.GetString("TEXT_Info_SimulationSingleStep"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(SimulationSingleStepText)); } }
        public string DefaultMessageText { get => Translator.GetString("TEXT_Info_DefaultMessage"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(DefaultMessageText)); } }
         
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
            PlayTooltip = Translator.GetString("TOOLTIP_Play");
            PauseTooltip = Translator.GetString("TOOLTIP_Pause");
            StopTooltip = Translator.GetString("TOOLTIP_Stop");
            StepTooltip = Translator.GetString("TOOLTIP_Step");
            SpeedTooltip = Translator.GetString("TOOLTIP_Speed");
            EingabeTooltip = Translator.GetString("TOOLTIP_Eingabe");
            UploadTooltip = Translator.GetString("TOOLTIP_Upload");
            Transformation1Tooltip = Translator.GetString("TOOLTIP_Transformation1");
            Transformation2Tooltip = Translator.GetString("TOOLTIP_Transformation2");
            Transformation3Tooltip = Translator.GetString("TOOLTIP_Transformation3");
            Transformation4Tooltip = Translator.GetString("TOOLTIP_Transformation4");
            Transformation5Tooltip = Translator.GetString("TOOLTIP_Transformation5");
            ViewText = Translator.GetString("TEXT_View");
            DiagramViewText = Translator.GetString("TEXT_DiagramView");
            TableViewText = Translator.GetString("TEXT_TableView");
            InvalidInputWordText = Translator.GetString("TEXT_InvalidInputWord");
            InvalidTapeNumberInDefinitionText = Translator.GetString("TEXT_InvalidTapeNumberInDefinition");
            ReadSymbolDoesNotExistText = Translator.GetString("TEXT_ReadSymbolDoesNotExist");
            SourceStateDoesNotExistText = Translator.GetString("TEXT_SourceStateDoesNotExist");
            StateDoesNotExistText = Translator.GetString("TEXT_StateDoesNotExist");
            StateExistsText = Translator.GetString("TEXT_StateExists");
            SymbolDoesNotExistText = Translator.GetString("TEXT_SymbolDoesNotExist");
            SymbolExistsText = Translator.GetString("TEXT_SymbolExists");
            TargetStateDoesNotExistText = Translator.GetString("TEXT_TargetStateDoesNotExist");
            TransitionDoesNotExistText = Translator.GetString("TEXT_TransitionDoesNotExist");
            TransitionExistsText = Translator.GetString("TEXT_TransitionExists");
            WriteSymbolDoesNotExistText = Translator.GetString("TEXT_WriteSymbolDoesNotExist");
            SimulationSuccessText = Translator.GetString("TEXT_Info_SimulationSuccess");
            SimulationFailureText = Translator.GetString("TEXT_Info_SimulationFailure");
            SimulationIsRunningText = Translator.GetString("TEXT_Info_SimulationIsRunning");
            SimulationIsPausedText = Translator.GetString("TEXT_Info_SimulationIsPaused");
            SimulationIsStoppedText = Translator.GetString("TEXT_Info_SimulationIsStopped");
            SimulationSingleStepText = Translator.GetString("TEXT_Info_SimulationSingleStep");
            DefaultMessageText = Translator.GetString("TEXT_Info_DefaultMessage");
        }
        #endregion

        public bool HighlightCurrentState { get; set; } = true;
        public bool IsSimulationRunning { get; set; } = true;
        public DiagramData DData { get; set; }

        public static ResourceManager Translator;
        private TuringMachine tm;
        public TuringMachine TM
        {
            get
            {
                return tm;
            }
            set
            {
                tm = value;
                OnPropertyChanged(nameof(TM));
            }
        }

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

            TM = new TuringMachine();

            DData = new DiagramData();
            InitResoureManager();

            timmy = new DispatcherTimer();
            timmy.Tick += Timmy_Tick;
            SetTimerInterval();

            UpdateInfo(MessageIdentification.DefaultMessage, DefaultMessageText);
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

            OnPropertyChanged(nameof(TM));
        }

        public void OnTmRefresh()
        {
            UpdateTapeData();
            RefreshTableData();
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
                UpdateInfo(MessageIdentification.SimulationIsPaused, SimulationIsPausedText);
            }
            else
            {
                startIsActive = true;
                StartVisibility = Visibility.Hidden;
                StopVisibility = Visibility.Visible;
                timmy.Start();
                UpdateInfo(MessageIdentification.SimulationIsRunning, SimulationIsRunningText);
            }
            OnTmRefresh();
        }

        private void Timmy_Tick(object sender, EventArgs e)
        {
            OnStepSimulation(true);
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
            OnTmRefresh();
            UpdateInfo(MessageIdentification.SimulationIsStopped, SimulationIsStoppedText);
        }

        private void OnStepSimulation(bool timerStep = false)
        {
            if (timerStep == false)
                UpdateInfo(MessageIdentification.SimulationSingleStep, SimulationSingleStepText);

            if (!TM.AdvanceState())
            {
                startIsActive = false;
                StartVisibility = Visibility.Visible;
                StopVisibility = Visibility.Hidden;
                timmy.Stop();
                CheckIfRunWasSuccessful();
            }
            OnTmRefresh();
        }

        private void CheckIfRunWasSuccessful()
        {
            if (TM.CheckIsEndState())
            {
                // Todo: Übersetzen
                UpdateInfo(MessageIdentification.SimulationSuccess, SimulationSuccessText);
            }
            else
            {
                UpdateInfo(MessageIdentification.SimulationFailure, SimulationFailureText);
            }
        }

        private void OnWriteTapeWord()
        {
            try
            {
                TM.WriteTapeWord(TapeWordInput);
            }
            catch (WordIsNoValidInputException)
            {
                QuickWarning(InvalidInputWordText);
                return;
            }
            LoadTapeContent();
        }

        private void OnTansformTuringMachine()
        {
            //TM.TansformTuringMachine();
            throw new NotImplementedException("OnTransformTuringMachine >> ViewModel");
        }

        private void OnTransformation1()
        {
            throw new NotImplementedException("OnTransformation1 >> ViewModel");
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
                try
                {
                    TM.ImportFromTextFile(importFileDialog.FileName);
                }
                catch (StateAlreadyExistsException)
                {
                    QuickWarning(StateExistsText);
                    return;
                }
                catch (TransitionAlreadyExistsException)
                {
                    QuickWarning(TransitionExistsText);
                    return;
                }
                catch (SourceStateOfTransitionDoesNotExistException)
                {
                    QuickWarning(SourceStateDoesNotExistText);
                    return;
                }
                catch (TargetStateOfTransitionDoesNotExistException)
                {
                    QuickWarning(TargetStateDoesNotExistText);
                    return;
                }
                catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
                {
                    QuickWarning(InvalidTapeNumberInDefinitionText);
                    return;
                }
                catch (ReadSymbolDoesNotExistException e)
                {
                    QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
                    return;
                }
                catch (WriteSymbolDoesNotExistException e)
                {
                    QuickWarning(WriteSymbolDoesNotExistText+ $" ({e.Message})");
                    return;
                }
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
                try 
                {
                    TM.ImportFromTextFile(loadExampleFileDialog.FileName);
                }
                catch (StateAlreadyExistsException)
                {
                    QuickWarning(StateExistsText);
                    return;
                }
                catch (TransitionAlreadyExistsException)
                {
                    QuickWarning(TransitionExistsText);
                    return;
                }
                catch (SourceStateOfTransitionDoesNotExistException)
                {
                    QuickWarning(SourceStateDoesNotExistText);
                    return;
                }
                catch (TargetStateOfTransitionDoesNotExistException)
                {
                    QuickWarning(TargetStateDoesNotExistText);
                    return;
                }
                catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
                {
                    QuickWarning(InvalidTapeNumberInDefinitionText);
                    return;
                }
                catch (ReadSymbolDoesNotExistException e)
                {
                    QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
                    return;
                }
                catch (WriteSymbolDoesNotExistException e)
                {
                    QuickWarning(WriteSymbolDoesNotExistText + $" ({e.Message})");
                    return;
                }
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
            AddStateDialog asd = new AddStateDialog($"q{TM.States.Count}", TM.States.Count < 1);
            if (asd.ShowDialog() == true)
            {
                string identifier = asd.Identfier;
                bool isStart = asd.IsStart;
                bool isAccepting = asd.IsAccepting;
                string comment = asd.Comment;

                try
                {
                    TM.AddState(new TuringState(identifier, comment, isStart, isAccepting));
                }
                catch (StateAlreadyExistsException)
                {
                    QuickWarning(StateExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void EditState(TuringState ts)
        {
            AddStateDialog asd = new AddStateDialog(ts);
            if (asd.ShowDialog() == true)
            {
                string identifier = asd.Identfier;
                bool isStart = asd.IsStart;
                bool isAccepting = asd.IsAccepting;
                string comment = asd.Comment;

                try
                {
                    TM.EditState(ts, new TuringState(identifier, comment, isStart, isAccepting));
                }
                catch (StateAlreadyExistsException)
                {
                    QuickWarning(StateExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void EditState(string turingStateAsString)
        {
            TuringState ts = null;

            foreach (var state in TM.States)
            {
                if (turingStateAsString == state.Identifier)
                    ts = state;
            }

            AddStateDialog asd = new AddStateDialog(ts);
            if (asd.ShowDialog() == true)
            {
                string identifier = asd.Identfier;
                bool isStart = asd.IsStart;
                bool isAccepting = asd.IsAccepting;
                string comment = asd.Comment;

                try
                {
                    TM.EditState(ts, new TuringState(identifier, comment, isStart, isAccepting));
                }
                catch (StateAlreadyExistsException)
                {
                    QuickWarning(StateExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void RemoveState(TuringState ts)
        {
            try
            {
                TM.RemoveState(ts);
            }
            catch (StateDoesNotExistException)
            {
                QuickWarning(StateDoesNotExistText);
                return;
            }
            OnTMChanged();
        }

        public void RemoveState(string turingStateAsString)
        {
            TuringState ts = null;

            foreach (var state in TM.States)
            {
                if (turingStateAsString == state.Identifier)
                    ts = state;
            }

            try
            {
                TM.RemoveState(ts);
            }
            catch (StateDoesNotExistException)
            {
                QuickWarning(StateDoesNotExistText);
                return;
            }
            OnTMChanged();
        }

        public void AddTransition(TuringState source = null, TuringState target = null)
        {
            AddTransitionDialog atd = new AddTransitionDialog(TM.States, source, target);
            if (atd.ShowDialog() == true)
            {
                //TODO: add checkboxes to decide whether new symbols should be in input alphabet
                atd.SymbolsWrite.ForEach((o) => { if (!TM.TapeSymbols.Contains(o)) { TM.AddSymbol(o, true); } });
                atd.SymbolsRead.ForEach((o) => { if (!TM.TapeSymbols.Contains(o)) { TM.AddSymbol(o, true); } });

                try
                {
                    TM.AddTransition(new TuringTransition(
                        atd.Source, atd.Target, atd.SymbolsRead,
                        atd.SymbolsWrite, atd.Directions, atd.Comment));
                }
                catch (TransitionAlreadyExistsException)
                {
                    QuickWarning(TransitionExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void EditTransition(TuringTransition tt)
        {
            AddTransitionDialog atd = new AddTransitionDialog(TM.States, tt);
            if (atd.ShowDialog() == true)
            {
                //TODO: add checkboxes to decide whether new symbols should be in input alphabet
                atd.SymbolsWrite.ForEach((o) => { if (!TM.TapeSymbols.Contains(o)) { TM.AddSymbol(o, true); } });
                atd.SymbolsRead.ForEach((o) => { if (!TM.TapeSymbols.Contains(o)) { TM.AddSymbol(o, true); } });

                try
                {
                    TM.EditTransition(tt, new TuringTransition(
                        atd.Source, atd.Target, atd.SymbolsRead,
                        atd.SymbolsWrite, atd.Directions, atd.Comment));
                }
                catch (TransitionAlreadyExistsException)
                {
                    QuickWarning(TransitionExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void EditTransition(string turingTransition, string symbolRead)
        {
            TuringTransition tt = null;

            foreach (var transtition in TM.Transitions)
            {
                if (transtition.Source.Identifier == turingTransition && transtition.SymbolsRead.Contains(symbolRead.ToCharArray()[0]))
                    tt = transtition;
            }

            AddTransitionDialog atd = new AddTransitionDialog(TM.States, tt);
            if (atd.ShowDialog() == true)
            {
                //TODO: add checkboxes to decide whether new symbols should be in input alphabet
                atd.SymbolsWrite.ForEach((o) => { if (!TM.TapeSymbols.Contains(o)) { TM.AddSymbol(o, true); } });
                atd.SymbolsRead.ForEach((o) => { if (!TM.TapeSymbols.Contains(o)) { TM.AddSymbol(o, true); } });

                try
                {
                    TM.EditTransition(tt, new TuringTransition(
                        atd.Source, atd.Target, atd.SymbolsRead,
                        atd.SymbolsWrite, atd.Directions, atd.Comment));
                }
                catch (TransitionAlreadyExistsException)
                {
                    QuickWarning(TransitionExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void RemoveTransition(TuringTransition tt)
        {
            try
            {
                TM.RemoveTransition(tt);
            }
            catch (TransitionDoesNotExistException)
            {
                QuickWarning(TransitionDoesNotExistText);
                return;
            }
            OnTMChanged();
        }

        public void RemoveTransition(string turingTransition, string symbolRead)
        {
            TuringTransition tt = null;

            foreach (var transtition in TM.Transitions)
            {
                if (transtition.Source.Identifier == turingTransition && transtition.SymbolsRead.Contains(symbolRead.ToCharArray()[0]))
                    tt = transtition;
            }

            try
            {
                TM.RemoveTransition(tt);
            }
            catch (TransitionDoesNotExistException)
            {
                QuickWarning(TransitionDoesNotExistText);
                return;
            }
            OnTMChanged();
        }

        public void AddSymbol()
        {
            AddSymbolDialog asd = new AddSymbolDialog();
            if (asd.ShowDialog() == true)
            {
                char symbol = asd.Symbol;
                bool isInputAlphabet = asd.IsInInput;

                try
                {
                    TM.AddSymbol(symbol, isInputAlphabet);
                }
                catch (SymbolAlreadyExistsException)
                {
                    QuickWarning(SymbolExistsText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void EditSymbol(string actSymbol, bool isInput)
        {
            EditSymbolDialog esd = new EditSymbolDialog(actSymbol.ToCharArray()[0], isInput);
            if (esd.ShowDialog() == true)
            {
                char symbol = esd.Symbol;
                bool isInputAlphabet = esd.IsInInput;

                try
                {
                    TM.EditSymbol(symbol, isInputAlphabet);
                }
                catch (SymbolDoesNotExistException)
                {
                    QuickWarning(SymbolDoesNotExistText);
                    return;
                }
                OnTMChanged();
            }
        }

        public void RemoveSymbol(string symbol)
        {
            try
            {
                TM.RemoveSymbol(symbol.ToCharArray()[0]);
            }
            catch (SymbolDoesNotExistException)
            {
                QuickWarning(SymbolDoesNotExistText);
                return;
            }
            OnTMChanged();
        }
    }
}

