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

namespace TMSim.UI
{
    public partial class AddStateDialog : Window
    {
        public AddStateDialog(string defaultIdentifier, bool forceStart = false)
        {
            InitializeComponent();
            ident_txt.Text = defaultIdentifier;
            var vm = (ViewModel)DataContext;

            if (forceStart)
            {
                start_chk.IsChecked = true;
                start_chk.IsEnabled = false;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            ident_txt.SelectAll();
            ident_txt.Focus();
        }

        private void ok_cmd_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Identfier
        {
            get { return ident_txt.Text; }
        }

        public bool IsStart
        {
            get { return (bool)start_chk.IsChecked; }
        }

        public bool IsAccepting
        {
            get { return (bool)accept_chk.IsChecked; }
        }
    }
}
