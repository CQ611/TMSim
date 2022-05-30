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
using System.Windows.Shapes;

namespace TMSim.UI
{
    public partial class EditSymbolDialog : Window
    {
        public EditSymbolDialog(string actSymbol, bool isInput)
        {
            InitializeComponent();
            symbol_txt.Text = actSymbol;
            isInput_chk.IsChecked = isInput;
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

        public string Symbol
        {
            get { return symbol_txt.Text; }
        }

        public bool IsInput
        {
            get { return (bool)isInput_chk.IsChecked; }
        }
    }
}
