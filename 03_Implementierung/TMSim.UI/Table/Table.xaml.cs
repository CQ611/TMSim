using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMSim.Core;

namespace TMSim.UI
{
    public partial class Table : UserControl
    {
        private List<RowHeader> rowHeaders = new List<RowHeader>();
        private List<ColumnHeader> columnHeaders = new List<ColumnHeader>();
        private List<TableCell> tableCells = new List<TableCell>();

        private ViewModel vm;

        #region Dependency properties
        public static DependencyProperty HighlightProperty = DependencyProperty.Register(
            "Highlight", typeof(bool), typeof(Table),
            new PropertyMetadata(false, HighlightPropertyChanged));

        private static void HighlightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Table)d).PropertyChangedCallback();
        }

        private void PropertyChangedCallback()
        {
        }
        #endregion

        public bool Highlight
        {
            get { return (bool)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        public Table()
        {
            InitializeComponent();

            RowHeader.EditStateEvent += RowHeader_EditStateEvent;
            RowHeader.RemoveStateEvent += RowHeader_RemoveStateEvent;
            ColumnHeader.EditSymbolEvent += ColumnHeader_EditSymbolEvent;
            ColumnHeader.RemoveSymbolEvent += ColumnHeader_RemoveSymbolEvent;
            TableCell.AddTransitionEvent += TableCell_AddTransitionEvent;
            TableCell.EditTransitionEvent += TableCell_EditTransitionEvent;
            TableCell.RemoveTransitionEvent += TableCell_RemoveTransitionEvent;

            vm = (ViewModel)DataContext;
            vm.ClearTableEvent += Vm_ClearTableEvent;
            vm.LoadTableEvent += Vm_LoadTableEvent;
            vm.RefreshActiveHighlightEvent += Vm_RefreshActiveHighlightEvent;
        }

        private void RowHeader_EditStateEvent(string identifier)
        {
            vm.EditState(identifier);
        }

        private void RowHeader_RemoveStateEvent(string identifier)
        {
            vm.RemoveState(identifier);
        }

        private void ColumnHeader_EditSymbolEvent(string symbol, bool isInput)
        {
            vm.EditSymbol(symbol, isInput);
        }

        private void ColumnHeader_RemoveSymbolEvent(string symbol)
        {
            vm.RemoveSymbol(symbol);
        }

        private void TableCell_AddTransitionEvent()
        {
            vm.AddTransition();
        }

        private void TableCell_EditTransitionEvent(string identifier, string symbolRead)
        {
            vm.EditTransition(identifier, symbolRead);
        }

        private void TableCell_RemoveTransitionEvent(string identifier, string symbolRead)
        {
            vm.RemoveTransition(identifier, symbolRead);
        }

        private void Vm_ClearTableEvent()
        {
            ClearTable();
        }

        private void Vm_LoadTableEvent(TuringMachine TM)
        {
            TM.TapeSymbols.ForEach(s => AddColumn(s.ToString(), TM.InputSymbols.Contains(s)));
            TM.States.ForEach(s => AddRow(s.Identifier, s == TM.StartState, TM.EndStates.Contains(s)));

            TM.Transitions.ForEach(t => OverwriteTransition(
                TM.States.FindIndex(x => x.Identifier == t.Source.Identifier) + 1,
                TM.TapeSymbols.FindIndex(x => x == t.SymbolsRead[0]) + 1,
                false,
                t));
        }

        private void Vm_RefreshActiveHighlightEvent(TuringMachine TM)
        {
            var highlightedCell = tableCells.Find(x => x.Highlight);
            if (highlightedCell != null)
                highlightedCell.Highlight = false;

            if(TM.CurrentTransition != null)
            {
                var cellForHighlight = tableCells.Find(x => x.State == TM.CurrentTransition.Source.Identifier && TM.CurrentTransition.SymbolsRead.Contains(x.SymbolRead.ToCharArray()[0]));
                if (cellForHighlight != null)
                    cellForHighlight.Highlight = true;
            }
        }

        private void OverwriteTransition(int row, int column, bool highlight, TuringTransition transition)
        {
            var state = transition.Source.Identifier;
            var direction = transition.MoveDirections[0].ToString();
            var symbolWrite = transition.SymbolsWrite[0].ToString();
            var symbolRead = transition.SymbolsRead[0].ToString();

            tableCells.Add(new TableCell(state, direction, symbolWrite, symbolRead, highlight));
            TableGrid.Children.Add(tableCells[tableCells.Count() - 1]);
            Grid.SetRow(tableCells[tableCells.Count() - 1], row);
            Grid.SetColumn(tableCells[tableCells.Count() - 1], column);
        }

        private void ClearTable()
        {
            rowHeaders.ForEach(x => TableGrid.Children.Remove(x));
            rowHeaders = new List<RowHeader>();

            columnHeaders.ForEach(x => TableGrid.Children.Remove(x));
            columnHeaders = new List<ColumnHeader>();

            tableCells.ForEach(x => TableGrid.Children.Remove(x));
            tableCells = new List<TableCell>();

            TableGrid.RowDefinitions.RemoveRange(1, TableGrid.RowDefinitions.Count - 2);
            TableGrid.ColumnDefinitions.RemoveRange(1, TableGrid.ColumnDefinitions.Count - 2);
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddState();
        }

        private void AddRow(string identifier, bool isStart, bool isAccepting)
        {
            TableGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(55) });

            Grid.SetRow(AddRowButton, TableGrid.RowDefinitions.Count() - 1);

            var newRowHeader = new RowHeader(isStart, isAccepting, identifier);
            rowHeaders.Add(newRowHeader);
            TableGrid.Children.Add(rowHeaders[rowHeaders.Count() - 1]);
            Grid.SetRow(rowHeaders[rowHeaders.Count() - 1], TableGrid.RowDefinitions.Count() - 2);

            SetTableCellsToNewRow();
        }

        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddSymbol();
        }

        private void AddColumn(string symbol, bool isInInputAlphabet)
        {
            TableGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(65) });

            Grid.SetColumn(AddColumnButton, TableGrid.ColumnDefinitions.Count() - 1);

            var newColumnHeader = new ColumnHeader(symbol, isInInputAlphabet);
            columnHeaders.Add(newColumnHeader);
            TableGrid.Children.Add(columnHeaders[columnHeaders.Count() - 1]);
            Grid.SetColumn(columnHeaders[columnHeaders.Count() - 1], TableGrid.ColumnDefinitions.Count() - 2);

            SetTableCellsToNewColumn();
        }

        private void SetTableCellsToNewRow()
        {
            for (int i = 0; i < columnHeaders.Count(); i++)
            {
                tableCells.Add(new TableCell());
                TableGrid.Children.Add(tableCells[tableCells.Count() - 1]);
                Grid.SetRow(tableCells[tableCells.Count() - 1], TableGrid.RowDefinitions.Count() - 2);
                Grid.SetColumn(tableCells[tableCells.Count() - 1], i + 1);
            }
        }

        private void SetTableCellsToNewColumn()
        {
            for (int i = 0; i < rowHeaders.Count(); i++)
            {
                tableCells.Add(new TableCell());
                TableGrid.Children.Add(tableCells[tableCells.Count() - 1]);
                Grid.SetRow(tableCells[tableCells.Count() - 1], i + 1);
                Grid.SetColumn(tableCells[tableCells.Count() - 1], TableGrid.ColumnDefinitions.Count() - 2);
            }
        }
    }
}
