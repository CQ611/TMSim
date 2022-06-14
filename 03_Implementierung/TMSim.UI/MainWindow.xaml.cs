using System;
using System.Collections.Generic;
using System.IO;
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
            LoadExampleMenue();
        }

        private void LoadExampleMenue()
        {
            var folderPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Examples");
            dirSearch(folderPath, ExamplesMenue);
        }

        public void dirSearch(string strDir, MenuItem targetItem)
        {
            try
            {
                foreach (string strFile in Directory.GetFiles(strDir))
                {
                    MenuItem x = new MenuItem();
                    x.Header = Path.GetFileName(strFile);
                    x.SetBinding(MenuItem.CommandProperty, new Binding("LoadExample"));
                    x.CommandParameter = strFile;
                    targetItem.Items.Add(x);
                }
                foreach (string strDirectory in Directory.GetDirectories(strDir))
                {
                    MenuItem y = new MenuItem();
                    y.Header = Path.GetFileName(strDirectory);
                    dirSearch(strDirectory, y);
                    targetItem.Items.Add(y);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
