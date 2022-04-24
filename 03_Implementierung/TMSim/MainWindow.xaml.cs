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

namespace TMSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void arrange_btn_Click(object sender, RoutedEventArgs e)
        {
            diagram.ArrangeDiagram();
        }

        private void randomize_btn_Click(object sender, RoutedEventArgs e)
        {
            diagram.GenerateTestDiagram(15, 20);
        }

        private void checkBox_Update(object sender, RoutedEventArgs e)
        {
            diagram.Animated = (bool)animate_chk.IsChecked;
        }
    }
}
