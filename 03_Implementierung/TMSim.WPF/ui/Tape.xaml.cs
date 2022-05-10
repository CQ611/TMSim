﻿using System;
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

namespace TMSim.WPF
{
    public partial class Tape : UserControl
    {
        [Bindable(true)]
        [Category("Action")]
        public RelayCommand Command { get; set; }        
        public string BandContent { get; set; }
        public Tape()
        {
            InitializeComponent();
        }
    }
}
