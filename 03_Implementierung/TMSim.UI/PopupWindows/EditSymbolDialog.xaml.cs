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

    public partial class EditSymbolDialog : Window
    {
        public EditSymbolDialog(char symbol, bool isInput)
        {
            InitializeComponent();
            var vm = (ViewModel)DataContext;

            Symbol = symbol;
            IsInInput = isInput;
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public char Symbol
        {
            get { return symbol.Content.ToString().ToCharArray()[0]; }
            set { symbol.Content = value.ToString(); }
        }

        public bool IsInInput
        {
            get { return (bool)isInInput_chk.IsChecked; }
            set { isInInput_chk.IsChecked = value; }
        }


    }
}
