using System.Collections.Generic;
using System.Windows;
using TMSim.Core;

namespace TMSim.UI
{
    public partial class AddEditTransitionDialog : Window
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

        private ViewModel vm;
        private List<ComboBoxItemWrapper> items = new List<ComboBoxItemWrapper>();


        public AddEditTransitionDialog(List<TuringState> states, List<char> symbols)
        {
            InitializeComponent();

            //States
            List<ComboBoxItemWrapper> StateItems = new List<ComboBoxItemWrapper>();
            states.ForEach((s) => StateItems.Add(new ComboBoxItemWrapper(s.Identifier, s)));
            sourceState_cmb.ItemsSource = StateItems;
            targetState_cmb.ItemsSource = StateItems;

            //Symbols
            readSymbols_lst.ItemsSource = symbols;
            writeSymbols_lst.ItemsSource = symbols;

            //Directions
            List<ComboBoxItemWrapper> DirectionItems = new List<ComboBoxItemWrapper>
            {
                new ComboBoxItemWrapper("→", TuringTransition.Direction.Right),
                new ComboBoxItemWrapper("←", TuringTransition.Direction.Left),
                new ComboBoxItemWrapper("•", TuringTransition.Direction.Neutral)
            };
            directions_cmb.ItemsSource = DirectionItems;

            vm = (ViewModel)DataContext;
        }

        public TuringState SourceState
        {
            get => (TuringState)sourceState_cmb.SelectedValue;
            set => sourceState_cmb.SelectedValue = value; //Fehlerprüfung?
        }

        public TuringState TargetState
        {
            get => (TuringState)targetState_cmb.SelectedValue;
            set => targetState_cmb.SelectedValue = value; //Fehlerprüfung?
        }

        public char? ReadSymbol
        {
            get => readSymbols_lst.SelectedIndex != -1 ? (char?)readSymbols_lst.SelectedItem : null;
            set => readSymbols_lst.SelectedItem = value; //Keine Fehlerprüfung ob Zeichen in Liste; nein? => nichts ausgewählt
        }

        public char? WriteSymbol
        {
            get => writeSymbols_lst.SelectedIndex != -1 ? (char?)writeSymbols_lst.SelectedItem : null;
            set => writeSymbols_lst.SelectedItem = value; //Keine Fehlerprüfung ob Zeichen in Liste; nein? => nichts ausgewählt
        }

        public TuringTransition.Direction? Direction
        {
            get => directions_cmb.SelectedIndex != -1 ? (TuringTransition.Direction?)directions_cmb.SelectedValue : null;
            set => directions_cmb.SelectedValue = value; //Keine Fehlerprüfung ob Richtung in Liste; nein? => nichts ausgewählt
        }

        public string Comment
        {
            get => comment_txt.Text;
            set => comment_txt.Text = value;
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void addSymbol_cmd_Click(object sender, RoutedEventArgs e)
        {
            vm.AddSymbol();
            readSymbols_lst.Items.Refresh();
            writeSymbols_lst.Items.Refresh();
        }
    }
}
