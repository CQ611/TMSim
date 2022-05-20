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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void switch1_cmd_Click(object sender, RoutedEventArgs e)
        {
            diagram.Visibility = Visibility.Hidden;
            table.Visibility = Visibility.Visible;
            switch1_cmd.IsEnabled = false;
            switch2_cmd.IsEnabled = true;
        }

        private void switch2_cmd_Click(object sender, RoutedEventArgs e)
        {
            diagram.Visibility = Visibility.Visible;
            table.Visibility = Visibility.Hidden;
            switch1_cmd.IsEnabled = true;
            switch2_cmd.IsEnabled = false;
        }
    }
}
