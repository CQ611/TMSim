using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TMSim.ui
{
    /// <summary>
    /// Interaction logic for Band.xaml
    /// </summary>
    public partial class Band : UserControl
    {
        [Bindable(true)]
        [Category("Action")]
        public RelayCommand Command { get; set; }        
        public string BandContent { get; set; }
        public Band()
        {
            InitializeComponent();
        }
    }
}
