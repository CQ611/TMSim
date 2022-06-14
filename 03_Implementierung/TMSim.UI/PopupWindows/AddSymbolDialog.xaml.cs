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

    public partial class AddSymbolDialog : Window
    {
        public AddSymbolDialog()
        {
            InitializeComponent();
            var vm = (ViewModel)DataContext;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            symbol_txt.SelectAll();
            symbol_txt.Focus();
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public char Symbol
        {
            get { return symbol_txt.Text[0]; }
        }

        public bool IsInInput
        {
            get { return (bool)isInInput_chk.IsChecked; }
        }

        public bool IsBlankChar
        {
            get { return (bool)isBlankChar_chk.IsChecked; }
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
