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
        public AddTransitionDialog(List<TuringState> states,
            TuringState source, TuringState target, int tapeCount = 1,
            string defaultSymbols = "")
        {
            InitializeComponent();

            TapeCount = tapeCount;
            List<ComboBoxItemWrapper> CBitems = new List<ComboBoxItemWrapper>();
            states.ForEach((s) => CBitems.Add(new ComboBoxItemWrapper(s.Identifier, s)));

            sourceState_cmb.ItemsSource = CBitems;
            targetState_cmb.ItemsSource = CBitems;
            sourceState_cmb.SelectedValue = source;
            targetState_cmb.SelectedValue = target;

            readSymbols_txt.Text = defaultSymbols;
            writeSymbols_txt.Text = defaultSymbols;

            List<ComboBoxItemWrapper> items = new List<ComboBoxItemWrapper>();
            items.Add(new ComboBoxItemWrapper("←",TuringTransition.Direction.Left));
            items.Add(new ComboBoxItemWrapper("→", TuringTransition.Direction.Right));
            items.Add(new ComboBoxItemWrapper("•", TuringTransition.Direction.Neutral));

            List<ComboBox> LVitems = new List<ComboBox>();
            for (int i = 0; i < tapeCount; i++)
            { 
                LVitems.Add(new ComboBox() {
                    ItemsSource = items,
                    SelectedValuePath = "InternalValue",
                    DisplayMemberPath = "DisplayValue" });
            }
            directions_lst.ItemsSource = LVitems;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            readSymbols_txt.SelectAll();
            readSymbols_txt.Focus();
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager t = ViewModel.Translator;
            if (SymbolsWrite.Count != TapeCount)
            {
                ViewModel.QuickWarning(t.GetString("TEXT_Warn_SymWrCount") +SymbolsWrite.Count + 
                    t.GetString("TEXT_Warn_SymCount") + TapeCount + t.GetString("TEXT_Tapes"));
                return;
            }

            if (SymbolsRead.Count != TapeCount)
            {
                ViewModel.QuickWarning(t.GetString("TEXT_Warn_SymReCount") + SymbolsRead.Count +
                      t.GetString("TEXT_Warn_SymCount") + TapeCount + t.GetString("TEXT_Tapes"));
                return;
            }

            this.DialogResult = true;
        }

        public TuringState Source { get { return (TuringState)sourceState_cmb.SelectedValue; } }
        public TuringState Target { get { return (TuringState)targetState_cmb.SelectedValue; } }
        public List<char> SymbolsRead { get { return new List<char>(readSymbols_txt.Text); } }
        public List<char> SymbolsWrite { get { return new List<char>(writeSymbols_txt.Text); } }
        public string Comment { get { return comment_txt.Text; } }        
        public List<TuringTransition.Direction> Directions {
            get {
                List<TuringTransition.Direction> dirs = new List<TuringTransition.Direction>();
                foreach (ComboBox box in directions_lst.Items)
                {
                    dirs.Add((TuringTransition.Direction)box.SelectedValue);
                }
                return dirs; } }
    }
}


