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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMSim.UI
{
    /// <summary>
    /// Interaction logic for RowHeader.xaml
    /// </summary>
    public partial class RowHeader : UserControl
    {
        public delegate void EditState(string identifier);
        public static event EditState EditStateEvent;

        public delegate void RemoveState(string identifier);
        public static event RemoveState RemoveStateEvent;

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

        private bool _highlight = false;
        public bool Highlight
        {
            get
            {
                return _highlight;
            }
            set
            {
                _highlight = value;
                SetBackground();
            }
        }

        public RowHeader(bool isStartState, bool isAcceptingState, string id, bool highlight)
        {
            InitializeComponent();
            IsStartState = isStartState;
            IsAcceptingState = isAcceptingState;
            Identifier = id;
            Highlight = highlight;
        }

        private void EditState_Click(object sender, RoutedEventArgs e)
        {
            EditStateEvent?.Invoke(Identifier);
        }

        private void RemoveState_Click(object sender, RoutedEventArgs e)
        {
            RemoveStateEvent?.Invoke(Identifier); 
        }

        private void SetBackground()
        {
            RowHeaderGrid.Background = Highlight ? Brushes.Yellow : Brushes.White;
        }
    }
}
