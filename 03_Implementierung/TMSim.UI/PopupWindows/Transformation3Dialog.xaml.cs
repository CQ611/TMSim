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
using System.Windows.Shapes;

namespace TMSim.UI.PopupWindows
{
    /// <summary>
    /// Interaktionslogik für Transformation3Dialog.xaml
    /// </summary>
    public partial class Transformation3Dialog : Window
    {
        public Transformation3Dialog()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            blank_txt.Focus();
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void blank_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length != 1) e.Handled = true;
        }
    }
}
