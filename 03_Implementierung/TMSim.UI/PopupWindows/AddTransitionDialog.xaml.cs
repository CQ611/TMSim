using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using TMSim.Core;

namespace TMSim.UI
{
    public partial class AddTransitionDialog : Window
    {
        private class ComboBoxItemWrapper
        {
            public ComboBoxItemWrapper(string display, object intern)
            {
                InternalValue = intern;
                DisplayValue = display;
            }
            public string DisplayValue { get; set; }
            public object InternalValue { get; set; }
        }

        private int TapeCount;
        private ViewModel vm;
        private List<ComboBoxItemWrapper> items = new List<ComboBoxItemWrapper>();

        public AddTransitionDialog(List<TuringState> states, TuringTransition tt, List<char> tapeSymbols, List<char> inputSymbols)
        {
            vm = (ViewModel)DataContext;

            Init(states, tt.Source, tt.Target, tt.SymbolsRead.Count);
            List<ComboBox> LVitems = new List<ComboBox>();
            for (int i = 0; i < tt.SymbolsRead.Count; i++)
            {
                ComboBox cb = new ComboBox()
                {
                    ItemsSource = items,
                    SelectedValuePath = "InternalValue",
                    DisplayMemberPath = "DisplayValue"
                };
                cb.SelectedValue = tt.MoveDirections[i];
                LVitems.Add(cb);
            }
            directions_lst.ItemsSource = LVitems;
            comment_txt.Text = tt.Comment;

            readSymbols_lst.ItemsSource = InputSymbols = inputSymbols;
            writeSymbols_lst.ItemsSource = TapeSymbols = tapeSymbols;
        }

        public AddTransitionDialog(List<TuringState> states, TuringState source, TuringState target, List<char> tapeSymbols, List<char> inputSymbols, int tapeCount = 1)
        {
            vm = (ViewModel)DataContext;

            Init(states, source, target, tapeCount);
            List<ComboBox> LVitems = new List<ComboBox>();
            for (int i = 0; i < tapeCount; i++)
            {
                ComboBox cb = new ComboBox()
                {
                    ItemsSource = items,
                    SelectedValuePath = "InternalValue",
                    DisplayMemberPath = "DisplayValue"
                };
                cb.SelectedIndex = 0;
                LVitems.Add(cb);
            }
            directions_lst.ItemsSource = LVitems;

            readSymbols_lst.ItemsSource = InputSymbols = inputSymbols;
            writeSymbols_lst.ItemsSource = TapeSymbols = tapeSymbols;
        }

        private void Init(List<TuringState> states, TuringState source, TuringState target, int tapeCount = 1)
        {
            InitializeComponent();

            TapeCount = tapeCount;
            List<ComboBoxItemWrapper> CBitems = new List<ComboBoxItemWrapper>();
            states.ForEach((s) => CBitems.Add(new ComboBoxItemWrapper(s.Identifier, s)));

            sourceState_cmb.ItemsSource = CBitems;
            targetState_cmb.ItemsSource = CBitems;
            sourceState_cmb.SelectedValue = source;
            targetState_cmb.SelectedValue = target;

            items.Add(new ComboBoxItemWrapper("→", TuringTransition.Direction.Right));
            items.Add(new ComboBoxItemWrapper("←", TuringTransition.Direction.Left));
            items.Add(new ComboBoxItemWrapper("•", TuringTransition.Direction.Neutral));
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager t = ViewModel.Translator;
            this.DialogResult = true;
        }

        public TuringState Source { get { return (TuringState)sourceState_cmb.SelectedValue; } }
        public TuringState Target { get { return (TuringState)targetState_cmb.SelectedValue; } }
        private List<char> _inputSymbols = new List<char>();
        private List<char> InputSymbols
        {
            get { return _inputSymbols; }
            set { _inputSymbols = value;  }
        }
        
        private List<char> _tapeSymbols = new List<char>();
        private List<char> TapeSymbols
        {
            get { return _tapeSymbols; }
            set { _tapeSymbols = value; }
        }

        public List<char> SymbolsRead { get { return new List<char>(readSymbols_lst.Text); } }
        public List<char> SymbolsWrite { get { return new List<char>(writeSymbols_lst.Text); } }


        public string Comment { get { return comment_txt.Text; } }
        public List<TuringTransition.Direction> Directions
        {
            get
            {
                List<TuringTransition.Direction> dirs = new List<TuringTransition.Direction>();
                foreach (ComboBox box in directions_lst.Items)
                {
                    dirs.Add((TuringTransition.Direction)box.SelectedValue);
                }
                return dirs;
            }
        }

        private void addSymbol_bt_Click(object sender, RoutedEventArgs e)
        {
            AddSymbolDialog asd = new AddSymbolDialog();
            if (asd.ShowDialog() == true)
            {
                char symbol = asd.Symbol;
                bool isInputAlphabet = asd.IsInInput;

                TapeSymbols.Add(symbol);
                if (isInputAlphabet) { InputSymbols.Add(symbol); }

                readSymbols_lst.ItemsSource = InputSymbols;
                readSymbols_lst.Items.Refresh();
                writeSymbols_lst.ItemsSource = TapeSymbols;
                writeSymbols_lst.Items.Refresh();
            }
        }
    }
}


