using System;
using System.Collections.Generic;
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

namespace TMSim.UI
{
    public partial class Table : UserControl
    {
        private List<RowHeader> rowHeaders = new List<RowHeader>();
        private List<ColumnHeader> columnHeaders = new List<ColumnHeader>();
        private List<TableCell> tableCells = new List<TableCell>();

        private int rowCount = 0;
        private int columnCount = 0;

        public Table()
        {
            InitializeComponent();
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(60);
            TableGrid.RowDefinitions.Add(newRow);

            Grid.SetRow(AddRemoveRow, TableGrid.RowDefinitions.Count() - 1);

            var stateName = "q" + rowCount;
            rowCount++;
            var newRowHeader = new RowHeader(false, false, stateName);
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

            Grid.SetColumn(AddRemoveColumn, TableGrid.ColumnDefinitions.Count() - 1);

            var symbol = columnCount.ToString();
            columnCount++;
            var newColumnHeader = new ColumnHeader(symbol, false);
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

        }

        private void RemoveColumnButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
