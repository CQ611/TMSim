using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
        public RelayCommand IncreaseSpeed { get; set; }
        public RelayCommand DecreaseSpeed { get; set; }
        public RelayCommand ToggleHighlight { get; set; }
        public RelayCommand ToggleDiagramView { get; set; }
        public RelayCommand ToggleTableView { get; set; }
        public RelayCommand PlayPause { get; set; }
        public RelayCommand Stop { get; set; }
        public RelayCommand Step { get; set; }
        public RelayCommand OpenHelpWindow { get; set; }
        public RelayCommand NextHelpPage { get; set; }
        public RelayCommand PreviousHelpPage { get; set; }

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
                RefreshDiagramData();
                RefreshTableData();
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

        private bool _startEnabled = false;
        public bool StartEnabled
        {
            get
            {
                return _startEnabled;
            }
            set
            {
                _startEnabled = value;
                OnPropertyChanged(nameof(StartEnabled));
            }
        }

        private bool _stopEnabled = false;
        public bool StopEnabled
        {
            get
            {
                return _stopEnabled;
            }
            set
            {
                _stopEnabled = value;
                OnPropertyChanged(nameof(StopEnabled));
            }
        }

        private bool _stepEnabled = false;
        public bool StepEnabled
        {
            get
            {
                return _stepEnabled;
            }
            set
            {
                _stepEnabled = value;
                OnPropertyChanged(nameof(StepEnabled));
            }
        }

        private bool _uploadTextEnabled = false;
        public bool UploadTextEnabled
        {
            get
            {
                return _uploadTextEnabled;
            }
            set
            {
                _uploadTextEnabled = value;
                OnPropertyChanged(nameof(UploadTextEnabled));
            }
        }

        private bool _menuElementEnabled = true;
        public bool MenuElementEnabled
        {
            get
            {
                return _menuElementEnabled;
            }
            set
            {
                _menuElementEnabled = value;
                OnPropertyChanged(nameof(MenuElementEnabled));
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
        public string DirectionText { get => Translator.GetString("TEXT_Direction"); set { OnPropertyChanged(nameof(DirectionText)); } }
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
        public string ImportFileIsNotValidText { get => Translator.GetString("TEXT_ImportFileIsNotValid"); set { OnPropertyChanged(nameof(ImportFileIsNotValidText)); } }
        public string InputAlphabetIsNoSubsetOfTapeAlphabetText { get => Translator.GetString("TEXT_InputAlphabetIsNoSubsetOfTapeAlphabet"); set { OnPropertyChanged(nameof(InputAlphabetIsNoSubsetOfTapeAlphabetText)); } }
        public string SimulationSuccessText { get => Translator.GetString("TEXT_Info_SimulationSuccess"); set { OnPropertyChanged(nameof(SimulationSuccessText)); } }
        public string SimulationFailureText { get => Translator.GetString("TEXT_Info_SimulationFailure"); set { OnPropertyChanged(nameof(SimulationFailureText)); } }
        public string SimulationIsRunningText { get => Translator.GetString("TEXT_Info_SimulationIsRunning"); set { OnPropertyChanged(nameof(SimulationIsRunningText)); } }
        public string SimulationIsPausedText { get => Translator.GetString("TEXT_Info_SimulationIsPaused"); set { OnPropertyChanged(nameof(SimulationIsPausedText)); } }
        public string SimulationIsStoppedText { get => Translator.GetString("TEXT_Info_SimulationIsStopped"); set { OnPropertyChanged(nameof(SimulationIsStoppedText)); } }
        public string SimulationSingleStepText { get => Translator.GetString("TEXT_Info_SimulationSingleStep"); set { OnPropertyChanged(nameof(SimulationSingleStepText)); } }
        public string DefaultMessageText { get => Translator.GetString("TEXT_Info_DefaultMessage"); set { TranslateCurrentInfo(); OnPropertyChanged(nameof(DefaultMessageText)); } }
        public string DefinitionTabelle { get => Translator.GetString("TEXT_DefinitionTabelle"); set { OnPropertyChanged(nameof(DefinitionTabelle)); } }
        public string DefinitionDiagramm { get => Translator.GetString("TEXT_DefinitionDiagramm"); set { OnPropertyChanged(nameof(DefinitionDiagramm)); } }
        public string InputWordWrittenOnTapeText { get => Translator.GetString("TEXT_Info_InputWordWrittenOnTape"); set { OnPropertyChanged(nameof(InputWordWrittenOnTapeText)); } }
        public string WarnTransformation4Text { get => Translator.GetString("TEXT_Warn_Transformation4"); set { OnPropertyChanged(nameof(WarnTransformation4Text)); } }
        public string WarnTransformation5Text { get => Translator.GetString("TEXT_Warn_Transformation5"); set { OnPropertyChanged(nameof(WarnTransformation5Text)); } }
        public string WarnMemoryText { get => Translator.GetString("TEXT_Warn_Memory"); set { OnPropertyChanged(nameof(WarnMemoryText)); } }
        public string TableText { get => Translator.GetString("TEXT_Table"); set { OnPropertyChanged(nameof(TableText)); } }
        public string EditSymbolText { get => Translator.GetString("TEXT_EditSymbol"); set { OnPropertyChanged(nameof(EditSymbolText)); } }
        public string RemoveSymbolText { get => Translator.GetString("TEXT_RemoveSymbol"); set { OnPropertyChanged(nameof(RemoveSymbolText)); } }
        public string AddSymbolText { get => Translator.GetString("TEXT_AddSymbol"); set { OnPropertyChanged(nameof(AddSymbolText)); } }
        public string AddTransitionText { get => Translator.GetString("TEXT_AddTransition"); set { OnPropertyChanged(nameof(AddTransitionText)); } }
        public string RemoveTransitionText { get => Translator.GetString("TEXT_RemoveTransition"); set { OnPropertyChanged(nameof(RemoveTransitionText)); } }
        public string EditTransitionText { get => Translator.GetString("TEXT_EditTransition"); set { OnPropertyChanged(nameof(EditTransitionText)); } }
        public string EditStateText { get => Translator.GetString("TEXT_EditState"); set { OnPropertyChanged(nameof(EditStateText)); } }
        public string RemoveStateText { get => Translator.GetString("TEXT_RemoveState"); set { OnPropertyChanged(nameof(RemoveStateText)); } }
        public string AddStateText { get => Translator.GetString("TEXT_AddState"); set { OnPropertyChanged(nameof(AddStateText)); } }
        public string NextBtnText { get => Translator.GetString("TEXT_NextBtn"); set { OnPropertyChanged(nameof(NextBtnText)); } }
        public string PrevBtnText { get => Translator.GetString("TEXT_PrevBtn"); set { OnPropertyChanged(nameof(PrevBtnText)); } }
        public string ArrangeText { get => Translator.GetString("TEXT_Arrange"); set { OnPropertyChanged(nameof(ArrangeText)); } }
        public string AnimateText { get => Translator.GetString("TEXT_Animate"); set { OnPropertyChanged(nameof(AnimateText)); } }
        public string PopupIsBlankCharText { get => Translator.GetString("TEXT_PopupIsBlankChar"); set { OnPropertyChanged(nameof(PopupIsBlankCharText)); } }
        public string WarnSymbolIsInputAndBlankText { get => Translator.GetString("TEXT_Warn_SymbolIsInputAndBlank"); set { OnPropertyChanged(nameof(WarnSymbolIsInputAndBlankText)); } }
        public string WarnBlankCharMustBeSetText { get => Translator.GetString("TEXT_Warn_BlankCharMustBeSet"); set { OnPropertyChanged(nameof(WarnBlankCharMustBeSetText)); } }
        public string WarnNoStartStateText { get => Translator.GetString("TEXT_Warn_NoStartState"); set { OnPropertyChanged(nameof(WarnNoStartStateText)); } }
        public string WarnUnsupportedLanguageText { get => Translator.GetString("TEXT_Warn_UnsupportedLanguage"); set { OnPropertyChanged(nameof(WarnUnsupportedLanguageText)); } }
        public string WarnSymbolAlreadyExistsText { get => Translator.GetString("TEXT_Warn_SymbolAlreadyExists"); set { OnPropertyChanged(nameof(WarnSymbolAlreadyExistsText)); } }
        public string WarnSymbolDoesNotExistText { get => Translator.GetString("TEXT_Warn_SymbolDoesNotExist"); set { OnPropertyChanged(nameof(WarnSymbolDoesNotExistText)); } }


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
            DefinitionTabelle = Translator.GetString("TEXT_DefinitionTabelle");
            DefinitionDiagramm = Translator.GetString("TEXT_DefinitionDiagramm");
            InputWordWrittenOnTapeText = Translator.GetString("TEXT_Info_InputWordWrittenOnTape");
            WarnTransformation4Text = Translator.GetString("TEXT_Warn_Transformation4");
            WarnTransformation5Text = Translator.GetString("TEXT_Warn_Transformation5");
            WarnMemoryText = Translator.GetString("TEXT_Warn_Memory");
            TableText = Translator.GetString("TEXT_Table");
            EditSymbolText = Translator.GetString("TEXT_EditSymbol");
            RemoveSymbolText = Translator.GetString("TEXT_RemoveSymbol");
            AddSymbolText = Translator.GetString("TEXT_AddSymbol");
            AddTransitionText = Translator.GetString("TEXT_AddTransition");
            EditTransitionText = Translator.GetString("TEXT_EditTransition");
            RemoveTransitionText = Translator.GetString("TEXT_RemoveTransition");
            EditStateText = Translator.GetString("TEXT_EditState");
            RemoveStateText = Translator.GetString("TEXT_RemoveState");
            NextBtnText = Translator.GetString("TEXT_NextBtn");
            PrevBtnText = Translator.GetString("TEXT_PrevBtn");
            AddStateText = Translator.GetString("TEXT_AddState");
            ArrangeText = Translator.GetString("TEXT_Arrange");
            AnimateText = Translator.GetString("TEXT_Animate");
            PopupIsBlankCharText = Translator.GetString("TEXT_PopupIsBlankChar");
            WarnSymbolIsInputAndBlankText = Translator.GetString("TEXT_Warn_SymbolIsInputAndBlank");
            WarnBlankCharMustBeSetText = Translator.GetString("TEXT_Warn_BlankCharMustBeSet");
            WarnNoStartStateText = Translator.GetString("TEXT_Warn_NoStartState");
            WarnUnsupportedLanguageText = Translator.GetString("TEXT_Warn_UnsupportedLanguage");
            WarnSymbolAlreadyExistsText = Translator.GetString("TEXT_Warn_SymbolAlreadyExists");
            WarnSymbolDoesNotExistText = Translator.GetString("TEXT_Warn_SymbolDoesNotExist");

            TranslateHelpWindow();
            TranslateCurrentInfo();
        }
        #endregion

        public bool HighlightCurrentState { get { return HighlightIsChecked; } }
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
            LoadExample = new RelayCommand((o) => { OnLoadExample(o); });
            ExitApplication = new RelayCommand((o) => { OnExitApplication(); });
            GermanLanguageSelected = new RelayCommand((o) => { OnGermanLanguageSelected(); });
            EnglishLanguageSelected = new RelayCommand((o) => { OnEnglishLanguageSelected(); });
            IncreaseSpeed = new RelayCommand((o) => { OnIncreaseSpeed(); });
            DecreaseSpeed = new RelayCommand((o) => { OnDecreaseSpeed(); });
            ToggleHighlight = new RelayCommand((o) => { OnToggleHighlight(); });
            ToggleDiagramView = new RelayCommand((o) => { OnToggleDiagramView(); });
            ToggleTableView = new RelayCommand((o) => { OnToggleTableView(); });
            PlayPause = new RelayCommand((o) => { OnPlayPause(); });
            Stop = new RelayCommand((o) => { OnStop(); });
            Step = new RelayCommand((o) => { OnStep(); });
            OpenHelpWindow = new RelayCommand((o) => { OnOpenHelpWindow(); });
            NextHelpPage = new RelayCommand((o) => { OnNextHelpPage(); });
            PreviousHelpPage = new RelayCommand((o) => { OnPreviousHelpPage(); });

            TM = new TuringMachine();

            DData = new DiagramData();
            InitResoureManager();

            timmy = new DispatcherTimer();
            timmy.Tick += Timmy_Tick;
            SetTimerInterval();

            UpdateInfo(MessageIdentification.DefaultMessage, DefaultMessageText);
            TranslateHelpWindow();
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

            try
            {
                FrameworkElement.LanguageProperty.OverrideMetadata(
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(
                            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            catch (Exception)
            {
                QuickWarning(WarnUnsupportedLanguageText);
            }
        }

        public void OnTMChanged()
        {
            UpdateDiagramData();
            UpdateTapeData();
            UpdateTableData();

            if (TM.InputSymbols.Count > 0)
                UploadTextEnabled = true;
            else
                UploadTextEnabled = false;

            OnPropertyChanged(nameof(TM));
        }

        public void OnTmRefresh()
        {
            UpdateTapeData();
            RefreshTableData();
            RefreshDiagramData();
        }

        private bool startIsActive = false;
        private bool simulationIsRunning = false;


        private void OnStartPauseSimulation()
        {
            if (simulationIsRunning == false)
            {
                simulationIsRunning = true;
                StopEnabled = true;
                UploadTextEnabled = false;
                MenuElementEnabled = false;
            }

            if (startIsActive)
            {
                startIsActive = false;
                StartVisibility = Visibility.Visible;
                StopVisibility = Visibility.Hidden;
                timmy.Stop();
                UpdateInfo(MessageIdentification.SimulationIsPaused, SimulationIsPausedText);
                StepEnabled = true;
            }
            else
            {
                startIsActive = true;
                StartVisibility = Visibility.Hidden;
                StopVisibility = Visibility.Visible;
                timmy.Start();
                UpdateInfo(MessageIdentification.SimulationIsRunning, SimulationIsRunningText);
                StepEnabled = false;
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
            StartEnabled = false;
            StopEnabled = false;
            StepEnabled = false;
            UploadTextEnabled = true;
            MenuElementEnabled = true;
            simulationIsRunning = false;
            timmy.Stop();
            OnTmRefresh();
            UpdateInfo(MessageIdentification.SimulationIsStopped, SimulationIsStoppedText);
        }

        private void OnStepSimulation(bool timerStep = false)
        {
            bool stateAdvanced = false;

            StopEnabled = true;
            UploadTextEnabled = false;
            MenuElementEnabled = false;

            if (timerStep == false)
                UpdateInfo(MessageIdentification.SimulationSingleStep, SimulationSingleStepText);

            try
            {
                stateAdvanced = TM.AdvanceState();
            }
            catch (NoStartStateException)
            {
                QuickWarning(WarnNoStartStateText);
            }
            catch (BlankCharMustBeSetException)
            {
                QuickWarning(WarnBlankCharMustBeSetText);
            }

            if (stateAdvanced == false)
            {
                startIsActive = false;
                StartVisibility = Visibility.Visible;
                StopVisibility = Visibility.Hidden;
                timmy.Stop();
                StartEnabled = false;
                StopEnabled = false;
                StepEnabled = false;
                UploadTextEnabled = true;
                MenuElementEnabled = true;
                CheckIfRunWasSuccessful();
            }
            OnTmRefresh();
        }

        private void CheckIfRunWasSuccessful()
        {
            if (TM.CheckIsEndState())
            {
                UpdateInfo(MessageIdentification.SimulationSuccess, SimulationSuccessText);
            }
            else
            {
                UpdateInfo(MessageIdentification.SimulationFailure, SimulationFailureText);
            }
        }

        private void OnWriteTapeWord()
        {
            TM.Reset();

            try
            {
                TM.WriteTapeWord(TapeWordInput);
            }
            catch (WordIsNoValidInputException)
            {
                QuickWarning(InvalidInputWordText);
            }
            LoadTapeContent();
            UpdateInfo(MessageIdentification.InputWordWrittenOnTape, InputWordWrittenOnTapeText);
            StartEnabled = true;
            StopEnabled = false;
            StepEnabled = true;
        }

        private void OnTansformTuringMachine() { }

        private void OnTransformation1()
        {
            TM = new Transformation1().Execute(TM);
            OnTMChanged();
        }

        private void OnTransformation2()
        {
            TM = new Transformation2().Execute(TM);
            OnTMChanged();
        }

        private void OnTransformation3()
        {
            Transformation3Dialog t3d = new Transformation3Dialog();
            if (t3d.ShowDialog() == true)
            {
                char newBlank = t3d.Blank;
                try
                {
                    TM = new Transformation3().Execute(TM, newBlank);
                }
                catch (SymbolAlreadyExistsException)
                {
                    QuickWarning(WarnSymbolAlreadyExistsText);
                }
                OnTMChanged();
            }
        }

        private void OnTransformation4()
        {
            if (new Transformation4().IsExecutable(TM))
            {
                TM = new Transformation4().Execute(TM);
                OnTMChanged();
            }
            else
            {
                QuickWarning(WarnTransformation4Text);
            }
        }

        private void OnTransformation5()
        {
            if (new Transformation5().IsExecutable(TM))
            {
                TM = new Transformation5().Execute(TM);
                OnTMChanged();
            }
            else
            {
                QuickWarning(WarnTransformation5Text);
            }
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
                }
                catch (TransitionAlreadyExistsException)
                {
                    QuickWarning(TransitionExistsText);
                }
                catch (SourceStateOfTransitionDoesNotExistException)
                {
                    QuickWarning(SourceStateDoesNotExistText);
                }
                catch (TargetStateOfTransitionDoesNotExistException)
                {
                    QuickWarning(TargetStateDoesNotExistText);
                }
                catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
                {
                    QuickWarning(InvalidTapeNumberInDefinitionText);
                }
                catch (TransitionNumberOfTapesIsInconsistentException)
                {
                    QuickWarning(InvalidTapeNumberInDefinitionText);
                }
                catch (ReadSymbolDoesNotExistException e)
                {
                    QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
                }
                catch (WriteSymbolDoesNotExistException e)
                {
                    QuickWarning(WriteSymbolDoesNotExistText + $" ({e.Message})");
                }
                catch (ImportFileIsNotValidException)
                {
                    QuickWarning(ImportFileIsNotValidText);
                }
                catch (InputAlphabetHasToBeASubsetOfTapeAlphabetException)
                {
                    QuickWarning(InputAlphabetIsNoSubsetOfTapeAlphabetText);
                }
                catch (SymbolCanNotBeInputAndBlankException)
                {
                    QuickWarning(WarnSymbolIsInputAndBlankText);
                }
                catch (SymbolAlreadyExistsException)
                {
                    QuickWarning(WarnSymbolAlreadyExistsText);
                }
                catch (SymbolDoesNotExistException)
                {
                    QuickWarning(WarnSymbolDoesNotExistText);
                }

                DeleteTapeContent();
                DData.ArangeFlag = true;
                OnTMChanged();
                UploadTextEnabled = true;
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
                try
                {
                    TM.ExportToTextFile(exportFileDialog.FileName);
                }
                catch (SystemException)
                {
                    QuickWarning(WarnMemoryText);
                }
            }
        }

        private void OnClearTuringMachine()
        {
            TM = new TuringMachine();
            OnTMChanged();
        }

        private void OnLoadExample(object o)
        {
            try
            {
                TM.ImportFromTextFile(o.ToString());
            }
            catch (StateAlreadyExistsException)
            {
                QuickWarning(StateExistsText);
            }
            catch (TransitionAlreadyExistsException)
            {
                QuickWarning(TransitionExistsText);
            }
            catch (SourceStateOfTransitionDoesNotExistException)
            {
                QuickWarning(SourceStateDoesNotExistText);
            }
            catch (TargetStateOfTransitionDoesNotExistException)
            {
                QuickWarning(TargetStateDoesNotExistText);
            }
            catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
            {
                QuickWarning(InvalidTapeNumberInDefinitionText);
            }
            catch (TransitionNumberOfTapesIsInconsistentException)
            {
                QuickWarning(InvalidTapeNumberInDefinitionText);
            }
            catch (ReadSymbolDoesNotExistException e)
            {
                QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
            }
            catch (WriteSymbolDoesNotExistException e)
            {
                QuickWarning(WriteSymbolDoesNotExistText + $" ({e.Message})");
            }
            catch (ImportFileIsNotValidException)
            {
                QuickWarning(ImportFileIsNotValidText);
            }
            catch (InputAlphabetHasToBeASubsetOfTapeAlphabetException)
            {
                QuickWarning(InputAlphabetIsNoSubsetOfTapeAlphabetText);
            }
            catch (SymbolCanNotBeInputAndBlankException)
            {
                QuickWarning(WarnSymbolIsInputAndBlankText);
            }
            catch (SymbolAlreadyExistsException)
            {
                QuickWarning(WarnSymbolAlreadyExistsText);
            }
            catch (SymbolDoesNotExistException)
            {
                QuickWarning(WarnSymbolDoesNotExistText);
            }

            DeleteTapeContent();
            DData.ArangeFlag = true;
            OnTMChanged();
            UploadTextEnabled = true;
        }

        private void UpdateDiagramData()
        {
            DData = UpdateDiagramData(DData, TM);
            //OnPropertyChanged(nameof(DData));
            DData.OnForcePropertyChanged();
        }

        private void OnOpenHelpWindow()
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Owner = Application.Current.MainWindow;
            helpWindow.Show();
        }

        private void OnNextHelpPage()
        {
            CurrentPageNumber++;

            if (CurrentPageNumber == 1)
                PreviousHelpPageAvailable = true;

            if (CurrentPageNumber == lastPageNumber)
                NextHelpPageAvailable = false;
        }

        private void OnPreviousHelpPage()
        {
            CurrentPageNumber--;

            if (CurrentPageNumber == lastPageNumber - 1)
                NextHelpPageAvailable = true;

            if (CurrentPageNumber == 0)
                PreviousHelpPageAvailable = false;
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

            RefreshTextFromUi();
            CurrentImageLanguage = language;
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

            EditState(ts);
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

            RemoveState(ts);
        }

        public void AddTransition(TuringState source = null, TuringState target = null)
        {
            AddEditTransitionDialog atd = new AddEditTransitionDialog(TM.States, TM.TapeSymbols)
            {
                SourceState = source,
                TargetState = target
            };

            if (atd.ShowDialog() == true)
            {
                if (atd.SourceState != null && //Fehlerprüfung noch in den Dialog auslagern!
                    atd.TargetState != null &&
                    atd.ReadSymbol != null &&
                    atd.WriteSymbol != null &&
                    atd.Direction != null)
                {
                    var tt = new TuringTransition(atd.SourceState,
                                                  atd.TargetState,
                                                  new List<char>() { (char)atd.ReadSymbol },
                                                  new List<char>() { (char)atd.WriteSymbol },
                                                  new List<TuringTransition.Direction>() { (TuringTransition.Direction)atd.Direction },
                                                  atd.Comment);

                    try
                    {
                        TM.AddTransition(tt);
                    }
                    catch (TransitionAlreadyExistsException)
                    {
                        QuickWarning(TransitionExistsText);
                    }
                    catch (ReadSymbolDoesNotExistException e)
                    {
                        QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
                    }
                    catch (WriteSymbolDoesNotExistException e)
                    {
                        QuickWarning(WriteSymbolDoesNotExistText + $" ({e.Message})");
                    }
                    catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
                    {
                        QuickWarning(InvalidTapeNumberInDefinitionText);
                    }
                    catch (SourceStateOfTransitionDoesNotExistException)
                    {
                        QuickWarning(SourceStateDoesNotExistText);
                    }
                    catch (TargetStateOfTransitionDoesNotExistException)
                    {
                        QuickWarning(TargetStateDoesNotExistText);
                    }
                    OnTMChanged();
                }
            }
        }

        public void AddTransition(string identifier, string symbolRead)
        {
            AddEditTransitionDialog atd = new AddEditTransitionDialog(TM.States, TM.TapeSymbols)
            {
                SourceState = TM.States.FirstOrDefault(o => o.Identifier == identifier),
                ReadSymbol = symbolRead.Length == 1 ? (char?)symbolRead[0] : null
            };

            if (atd.ShowDialog() == true)
            {
                if (atd.SourceState != null && //Fehlerprüfung noch in den Dialog auslagern!
                    atd.TargetState != null &&
                    atd.ReadSymbol != null &&
                    atd.WriteSymbol != null &&
                    atd.Direction != null)
                {
                    var tt = new TuringTransition(atd.SourceState,
                                                  atd.TargetState,
                                                  new List<char>() { (char)atd.ReadSymbol },
                                                  new List<char>() { (char)atd.WriteSymbol },
                                                  new List<TuringTransition.Direction>() { (TuringTransition.Direction)atd.Direction },
                                                  atd.Comment);

                    try
                    {
                        TM.AddTransition(tt);
                    }
                    catch (TransitionAlreadyExistsException)
                    {
                        QuickWarning(TransitionExistsText);
                    }
                    catch (ReadSymbolDoesNotExistException e)
                    {
                        QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
                    }
                    catch (WriteSymbolDoesNotExistException e)
                    {
                        QuickWarning(WriteSymbolDoesNotExistText + $" ({e.Message})");
                    }
                    catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
                    {
                        QuickWarning(InvalidTapeNumberInDefinitionText);
                    }
                    catch (SourceStateOfTransitionDoesNotExistException)
                    {
                        QuickWarning(SourceStateDoesNotExistText);
                    }
                    catch (TargetStateOfTransitionDoesNotExistException)
                    {
                        QuickWarning(TargetStateDoesNotExistText);
                    }
                    OnTMChanged();
                }
            }
        }

        public void EditTransition(TuringTransition tt)
        {
            AddEditTransitionDialog atd = new AddEditTransitionDialog(TM.States, TM.TapeSymbols)
            {
                SourceState = tt.Source,
                TargetState = tt.Target,
                ReadSymbol = tt.SymbolsRead[0],
                WriteSymbol = tt.SymbolsWrite[0],
                Direction = tt.MoveDirections[0],
                Comment = tt.Comment
            };

            if (atd.ShowDialog() == true)
            {
                if (atd.SourceState != null && //Fehlerprüfung noch in den Dialog auslagern!
                    atd.TargetState != null &&
                    atd.ReadSymbol != null &&
                    atd.WriteSymbol != null &&
                    atd.Direction != null)
                {
                    // This function calls TM.RemoveTransition and TM.AddTransition so their exceptions must be
                    // caught here too
                    try
                    {
                        TM.EditTransition(tt, new TuringTransition(atd.SourceState,
                                                                   atd.TargetState,
                                                                   new List<char>() { (char)atd.ReadSymbol },
                                                                   new List<char>() { (char)atd.WriteSymbol },
                                                                   new List<TuringTransition.Direction>() { (TuringTransition.Direction)atd.Direction },
                                                                   atd.Comment));
                    }
                    catch (TransitionAlreadyExistsException)
                    {
                        QuickWarning(TransitionExistsText);
                    }
                    catch (ReadSymbolDoesNotExistException e)
                    {
                        QuickWarning(ReadSymbolDoesNotExistText + $" ({e.Message})");
                    }
                    catch (WriteSymbolDoesNotExistException e)
                    {
                        QuickWarning(WriteSymbolDoesNotExistText + $" ({e.Message})");
                    }
                    catch (NumberOfTapesDoesNotMatchToTransitionDefinitionException)
                    {
                        QuickWarning(InvalidTapeNumberInDefinitionText);
                    }
                    catch (SourceStateOfTransitionDoesNotExistException)
                    {
                        QuickWarning(SourceStateDoesNotExistText);
                    }
                    catch (TargetStateOfTransitionDoesNotExistException)
                    {
                        QuickWarning(TargetStateDoesNotExistText);
                    }
                    catch (TransitionDoesNotExistException)
                    {
                        QuickWarning(TransitionDoesNotExistText);
                    }
                    OnTMChanged();
                }
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

            EditTransition(tt);
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

            RemoveTransition(tt);
        }

        public void AddSymbol()
        {
            AddEditRemoveSymbolDialog aersd = new AddEditRemoveSymbolDialog();
            aersd.AddModus = true;
            if (aersd.ShowDialog() == true)
            {
                char symbol = aersd.Symbol;
                bool isInputAlphabet = aersd.IsInInput;
                bool isBlankChar = aersd.IsBlankChar;

                try
                {
                    TM.AddSymbol(symbol, isInputAlphabet, isBlankChar);
                }
                catch (SymbolAlreadyExistsException)
                {
                    QuickWarning(SymbolExistsText);
                }
                catch (SymbolCanNotBeInputAndBlankException)
                {
                    QuickWarning(WarnSymbolIsInputAndBlankText);
                }
                OnTMChanged();
            }
        }

        public void EditSymbol(string symbol = " ", bool isInput = false, bool isBlank = false)
        {
            AddEditRemoveSymbolDialog aersd = new AddEditRemoveSymbolDialog();
            aersd.Symbol = symbol.ToCharArray()[0];
            aersd.TapeSymbols = TM.TapeSymbols;
            aersd.IsInInput = isInput;
            aersd.IsBlankChar = isBlank;
            aersd.EditModus = true;

            if (aersd.ShowDialog() == true)
            {
                char actSymbol = aersd.Symbol;
                bool isInputAlphabet = aersd.IsInInput;
                bool isBlankChar = aersd.IsBlankChar;

                try
                {
                    TM.EditSymbol(actSymbol, isInputAlphabet, isBlankChar);
                }
                catch (SymbolDoesNotExistException)
                {
                    QuickWarning(SymbolDoesNotExistText);
                }
                catch (SymbolCanNotBeInputAndBlankException)
                {
                    QuickWarning(WarnSymbolIsInputAndBlankText);
                }
                OnTMChanged();
            }
        }

        public void RemoveSymbol(string symbol = " ")
        {
            AddEditRemoveSymbolDialog aersd = new AddEditRemoveSymbolDialog();
            aersd.Symbol = symbol.ToCharArray()[0];
            aersd.TapeSymbols = TM.TapeSymbols;
            aersd.IsInInput = TM.InputSymbols.Contains(symbol.ToCharArray()[0]);
            aersd.IsBlankChar = TM.BlankChar.Equals(symbol.ToCharArray()[0]);
            aersd.RemoveModus = true;

            if (aersd.ShowDialog() == true)
            {
                try
                {
                    TM.RemoveSymbol(aersd.Symbol);
                }
                catch (SymbolDoesNotExistException)
                {
                    QuickWarning(SymbolDoesNotExistText);
                }
                catch (SymbolCanNotBeInputAndBlankException)
                {
                    QuickWarning(WarnSymbolIsInputAndBlankText);
                }
                OnTMChanged();
            }
        }

        private void OnDecreaseSpeed()
        {
            if (TapeVelocity + 100 < 2000) TapeVelocity += 100;
            else TapeVelocity = 2000;
        }

        private void OnIncreaseSpeed()
        {
            if (TapeVelocity - 100 > 0) TapeVelocity -= 100;
            else TapeVelocity = 0;
        }

        private void OnToggleHighlight()
        {
            HighlightIsChecked = !HighlightIsChecked;
        }

        private void OnToggleDiagramView()
        {
            if (TableViewIsChecked) DiagramViewIsChecked = !DiagramViewIsChecked;
        }

        private void OnToggleTableView()
        {
            if (DiagramViewIsChecked) TableViewIsChecked = !TableViewIsChecked;
        }

        private void OnPlayPause()
        {
            if (StartEnabled) OnStartPauseSimulation();
        }

        private void OnStop()
        {
            if (StopEnabled) OnStopSimulation();
        }

        private void OnStep()
        {
            if (StepEnabled) OnStepSimulation();
        }
    }
}

