using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMSim.UI
{

    public partial class AddEditRemoveSymbolDialog : Window
    {
        private bool _addModus = false;
        public bool AddModus
        {
            get { return _addModus; }
            set
            {
                _addModus = value;
                if (value)
                {
                    EditModus = false;
                    RemoveModus = false;
                }
                SetDialogItems();
            }
        }

        private bool _editModus = false;
        public bool EditModus
        {
            get { return _editModus; }
            set
            {
                _editModus = value;
                if (value)
                {
                    AddModus = false;
                    RemoveModus = false;
                }
                SetDialogItems();
            }
        }

        private bool _removeModus = false;
        public bool RemoveModus
        {
            get { return _removeModus; }
            set
            {
                _removeModus = value;
                if (value)
                {
                    AddModus = false;
                    EditModus = false;
                }
                SetDialogItems();
            }
        }
        private char _symbol;
        public char Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        public bool IsInInput
        {
            get { return (bool)isInInput_chk.IsChecked; }
            set { isInInput_chk.IsChecked = value; }
        }

        public bool IsBlankChar
        {
            get { return (bool)isBlankChar_chk.IsChecked; }
            set { isBlankChar_chk.IsChecked = value; }
        }

        private List<char> _tapeSymbols = new List<char>();
        public List<char> TapeSymbols
        {
            get { return _tapeSymbols; }
            set
            {
                _tapeSymbols = value;
            }
        }

        public AddEditRemoveSymbolDialog()
        {
            InitializeComponent();
            var vm = (ViewModel)DataContext;
        }

        private void SetDialogItems()
        {
            if (AddModus)
            {
                symbol_txt.Visibility = Visibility.Visible;
                symbol_cb.Visibility = Visibility.Hidden;
                isInInput_chk.IsEnabled = true;
                isBlankChar_chk.IsEnabled = true;
                ok_cmd.IsEnabled = false;
            }
            else if (EditModus)
            {
                symbol_txt.Visibility = Visibility.Hidden;
                symbol_cb.Visibility = Visibility.Visible;
                symbol_cb.Items.Clear();
                foreach (var symbol in TapeSymbols)
                {
                    symbol_cb.Items.Add(symbol.ToString());
                }
                symbol_cb.SelectedItem = Symbol.ToString();
                isInInput_chk.IsEnabled = true;
                isBlankChar_chk.IsEnabled = true;
                ok_cmd.IsEnabled = true;
            }
            else if (RemoveModus)
            {
                symbol_txt.Visibility = Visibility.Hidden;
                symbol_cb.Visibility = Visibility.Visible;
                symbol_cb.Items.Clear();
                foreach (var symbol in TapeSymbols)
                {
                    symbol_cb.Items.Add(symbol.ToString());
                }
                symbol_cb.SelectedItem = Symbol.ToString();
                isInInput_chk.IsEnabled = false;
                isBlankChar_chk.IsEnabled = false;
                ok_cmd.IsEnabled = true;
            }
            else
            {
                return;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            symbol_txt.SelectAll();
            symbol_txt.Focus();
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            if (AddModus)
                Symbol = symbol_txt.Text[0];
            else
            {
                if (symbol_cb.SelectedItem != null)
                    Symbol = symbol_cb.SelectedItem.ToString().ToCharArray()[0];
            }
            this.DialogResult = true;
        }

        private void symbol_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ok_cmd.IsEnabled = symbol_txt.Text != "";
        }

        private void symbol_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length != 1) e.Handled = true;
        }
    }
}
