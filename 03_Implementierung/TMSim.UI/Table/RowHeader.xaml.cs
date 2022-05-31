﻿using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for RowHeader.xaml
    /// </summary>
    public partial class RowHeader : UserControl
    {
        private bool _isStartState;
        public bool IsStartState
        {
            get
            {
                return _isStartState;
            }
            set
            {
                _isStartState = value;
                CheckboxIsStart.IsChecked = _isStartState;
            }
        }

        private bool _isAcceptingState;
        public bool IsAcceptingState
        {
            get
            {
                return _isAcceptingState;
            }
            set
            {
                _isAcceptingState = value;
                CheckboxIsEnd.IsChecked = _isAcceptingState;
            }
        }

        private string _identifier;
        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                _identifier = value;
                ID.Content = _identifier;
            }
        }

        public RowHeader(bool isStartState, bool isAcceptingState, string id)
        {
            InitializeComponent();
            IsStartState = isStartState;
            IsAcceptingState = isAcceptingState;
            Identifier = id;
        }

        private void EditState_Click(object sender, RoutedEventArgs e)
        {
            var asd = new EditStateDialog(Identifier, IsStartState, IsAcceptingState);
            if (asd.ShowDialog() == true)
            {
                Identifier = asd.Identfier;
                IsStartState = asd.IsStart;
                IsAcceptingState = asd.IsAccepting;
                string comment = "DEBUG: TODO: get comment from popup";
                //TODO: get comment from popup
            }
        }

        private void RemoveState_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}