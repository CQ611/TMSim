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

        private int rowCount = 0;
        private int columnCount = 0;

        private int thomascount = 0;

        #region Dependency properties
        public static DependencyProperty HighlightProperty = DependencyProperty.Register(
            "Highlight", typeof(bool), typeof(Table),
            new PropertyMetadata(false, HighlightPropertyChanged));

        public static DependencyProperty TuringmachineProperty = DependencyProperty.Register(
            "Turingmachine", typeof(TuringMachine), typeof(Table),
            new PropertyMetadata(null, TuringmachinePropertyChanged));

        private static void HighlightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //dodo Aktion implementieren
            ((Table)d).PropertyChangedCallback();
        }

        private static void TuringmachinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //dodo Aktion implementieren
            ((Table)d).TuringmachinePropertyChangedCallback();   
        }

        private void PropertyChangedCallback()
        {
            //brauchen wir nicht?
            //InvalidateVisual();
            Trace.WriteLine("Highlight State Table: " + Highlight.ToString());
        }

        private void TuringmachinePropertyChangedCallback()
        {
            //brauchen wir nicht?
            //InvalidateVisual();
            Trace.WriteLine("Turingmachine Changed count: " + thomascount.ToString());
        }
        #endregion

        public bool Highlight
        {
            get { return (bool)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        public TuringMachine Turingmachine
        {
            get { return (TuringMachine)GetValue(TuringmachineProperty); }
            set { SetValue(TuringmachineProperty, value); }
        }

        public Table()
        {
            InitializeComponent();

            var vm = (ViewModel)DataContext;
            vm.ClearTableEvent += Vm_ClearTableEvent;
            vm.LoadTableEvent += Vm_LoadTableEvent; ;
        }

        private void Vm_LoadTableEvent(
            List<char> TapeSymbols,
            List<char> InputSymbols,
            List<Core.TuringState> States,
            Core.TuringState StartState,
            List<Core.TuringState> EndStates,
            List<Core.TuringTransition> Transitions,
            Core.TuringState CurrentState,
            Core.TuringTransition CurrentTransition)
        {
            //AddColumns
            foreach (var symbol in TapeSymbols)
            {
                bool ifInputSymbol = InputSymbols.Contains(symbol);
                AddColumn(symbol.ToString(), ifInputSymbol);
            }

            //AddRows
            foreach (var state in States)
            {
                bool isStart = state == StartState;
                bool isAccepting = EndStates.Contains(state);
                AddRow(state.Identifier, isStart, isAccepting);
            }

            //AddTransitions
            foreach (var transition in Transitions)
            {
                int row = 1;
                int column = 1;
                int i = 1;
                foreach (var state in States)
                {
                    if (state.Identifier == transition.Source.Identifier)
                        row = i;
                    i++;
                }
                i = 1;

                foreach (var symbol in TapeSymbols)
                {
                    if (symbol.ToString() == transition.SymbolsRead[0].ToString())
                        column = i;
                    i++;
                }

                OverwriteTransition(row, column, transition.SymbolsRead, transition.SymbolsWrite, transition.MoveDirections, transition.Target);
            }

        }

        private void OverwriteTransition(
            int row,
            int column,
            List<char> symbolsRead,
            List<char> symbolsWrite,
            List<Core.TuringTransition.Direction> moveDirections,
            Core.TuringState target)
        {
            tableCells.Add(new TableCell(target.Identifier, moveDirections[0].ToString(), symbolsWrite[0].ToString(), symbolsRead[0].ToString()));
            TableGrid.Children.Add(tableCells[tableCells.Count() - 1]);
            Grid.SetRow(tableCells[tableCells.Count() - 1], row);
            Grid.SetColumn(tableCells[tableCells.Count() - 1], column);
        }

        private void Vm_ClearTableEvent()
        {
            ClearTable();
        }

        private void ClearTable()
        {
            foreach (var row in rowHeaders)
            {
                TableGrid.Children.Remove(row);
            }
            rowHeaders = new List<RowHeader>();

            foreach (var column in columnHeaders)
            {
                TableGrid.Children.Remove(column);
            }
            columnHeaders = new List<ColumnHeader>();

            foreach (var cell in tableCells)
            {
                TableGrid.Children.Remove(cell);
            }
            tableCells = new List<TableCell>();

            List<RowDefinition> rowsToDelete = new List<RowDefinition>();
            foreach (var rowDef in TableGrid.RowDefinitions)
            {
                if (rowDef.Name != row1.Name && rowDef.Name != row2.Name)
                    rowsToDelete.Add(rowDef);
            }
            foreach (var row in rowsToDelete)
                TableGrid.RowDefinitions.Remove(row);

            List<ColumnDefinition> columnsToDelete = new List<ColumnDefinition>();
            foreach (var columnDef in TableGrid.ColumnDefinitions)
            {
                if (columnDef.Name != column1.Name && columnDef.Name != column2.Name)
                    columnsToDelete.Add(columnDef);

            }
            foreach (var column in columnsToDelete)
                TableGrid.ColumnDefinitions.Remove(column);

        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(60);
            TableGrid.RowDefinitions.Add(newRow);

            Grid.SetRow(AddRowButton, TableGrid.RowDefinitions.Count() - 1);

            var stateName = "q" + rowCount;
            rowCount++;
            var newRowHeader = new RowHeader(false, false, stateName);
            rowHeaders.Add(newRowHeader);
            TableGrid.Children.Add(rowHeaders[rowHeaders.Count() - 1]);
            Grid.SetRow(rowHeaders[rowHeaders.Count() - 1], TableGrid.RowDefinitions.Count() - 2);

            SetTableCellsToNewRow();
        }

        private void AddRow(string identifier, bool isStart, bool isAccepting)
        {
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(60);
            TableGrid.RowDefinitions.Add(newRow);

            Grid.SetRow(AddRowButton, TableGrid.RowDefinitions.Count() - 1);

            var newRowHeader = new RowHeader(isStart, isAccepting, identifier);
            rowHeaders.Add(newRowHeader);
            TableGrid.Children.Add(rowHeaders[rowHeaders.Count() - 1]);
            Grid.SetRow(rowHeaders[rowHeaders.Count() - 1], TableGrid.RowDefinitions.Count() - 2);

            SetTableCellsToNewRow();
        }

        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            var newColumn = new ColumnDefinition();
            newColumn.Width = new GridLength(100);
            TableGrid.ColumnDefinitions.Add(newColumn);

            Grid.SetColumn(AddColumnButton, TableGrid.ColumnDefinitions.Count() - 1);

            var symbol = columnCount.ToString();
            columnCount++;
            var newColumnHeader = new ColumnHeader(symbol, false);
            columnHeaders.Add(newColumnHeader);
            TableGrid.Children.Add(columnHeaders[columnHeaders.Count() - 1]);
            Grid.SetColumn(columnHeaders[columnHeaders.Count() - 1], TableGrid.ColumnDefinitions.Count() - 2);

            SetTableCellsToNewColumn();
        }

        private void AddColumn(string symbol, bool isInInputAlphabet)
        {
            var newColumn = new ColumnDefinition();
            newColumn.Width = new GridLength(100);
            TableGrid.ColumnDefinitions.Add(newColumn);

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

        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTable();
        }

        private void RemoveColumnButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTable();
        }
    }
}
