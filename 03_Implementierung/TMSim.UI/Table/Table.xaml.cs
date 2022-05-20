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
        public Table()
        {
            InitializeComponent();
        }

        private List<List<string>> tableMatrix = new List<List<string>>();

        private void ButtonAddRow_Click(object sender, RoutedEventArgs e)
        {
            TuringTable.RowGroups[0].Rows.Add(new TableRow());
            var actualRow = TuringTable.RowGroups[0].Rows.Count() - 1;
            TableRow currentRow = TuringTable.RowGroups[0].Rows[actualRow];
            if (actualRow % 2 == 1)
                currentRow.Background = Brushes.Silver;
            else
                currentRow.Background = Brushes.Transparent;
            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Normal;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Neue Zeile"))));

        }

        private void ButtonAddColumn_Click(object sender, RoutedEventArgs e)
        {
            TuringTable.Columns.Add(new TableColumn());
            TuringTable.RowGroups[0].Rows[0].Cells.Add(new TableCell(new Paragraph(new Run("Neue Spalte"))));
        }
    }
}
