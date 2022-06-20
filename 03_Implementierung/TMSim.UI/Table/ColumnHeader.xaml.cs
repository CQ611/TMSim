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
        public delegate void EditSymbol(string symbol, bool isInput, bool isBlankChar);
        public static event EditSymbol EditSymbolEvent;

        public delegate void RemoveSymbol(string symbol);
        public static event RemoveSymbol RemoveSymbolEvent;

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
                LabelSymbol.FontWeight = value ? FontWeights.Bold : FontWeights.Normal;
            }
        }

        private bool _isBlankChar;
        public bool IsBlankChar
        {
            get
            {
                return _isBlankChar;
            }
            set
            {
                _isBlankChar = value;
                LabelSymbol.Foreground = value ? Brushes.Red : Brushes.Black;
            }
        }
            

        public ColumnHeader(string symbol, bool isInput, bool isBlankChar)
        {
            InitializeComponent();
            Symbol = symbol;
            IsInput = isInput;
            IsBlankChar = isBlankChar;
        }

        private void EditSymbol_Click(object sender, RoutedEventArgs e)
        {
            EditSymbolEvent?.Invoke(Symbol, IsInput, IsBlankChar);
        }

        private void RemoveSymbol_Click(object sender, RoutedEventArgs e)
        {
            RemoveSymbolEvent.Invoke(Symbol);
        }
    }
}
