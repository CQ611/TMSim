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
    /// <summary>
    /// Interaction logic for ColumnHeader.xaml
    /// </summary>
    public partial class ColumnHeader : UserControl
    {
        private string _symbol;
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
                LabelSymbol.Content = _symbol;
            }
        }

        private bool _isInput;
        public bool IsInput
        {
            get
            {
                return _isInput;
            }
            set
            {
                _isInput = value;
            }
        }

        public ColumnHeader(string symbol, bool isInput)
        {
            InitializeComponent();
            Symbol = symbol;
            IsInput = isInput;
        }

        private void EditSymbol_Click(object sender, RoutedEventArgs e)
        {
            var esd = new EditSymbolDialog(Symbol, IsInput);
            if (esd.ShowDialog() == true)
            {
                Symbol = esd.Symbol;
                IsInput = esd.IsInput;
                string comment = "DEBUG: TODO: get comment from popup";
                //TODO: get comment from popup
            }
        }

        private void RemoveSymbol_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
