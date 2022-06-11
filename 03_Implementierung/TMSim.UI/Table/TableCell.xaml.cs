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
    /// Interaction logic for TableCell.xaml
    /// </summary>
    public partial class TableCell : UserControl
    {
        public delegate void AddTransition();
        public static event AddTransition AddTransitionEvent;

        public delegate void EditTransition(string identifier, string symbolRead);
        public static event EditTransition EditTransitionEvent;

        public delegate void RemoveTransition(string identifier, string symbolRead);
        public static event RemoveTransition RemoveTransitionEvent;

        private string _sourceState;
        public string SourceState
        {
            get
            {
                return _sourceState;
            }
            set
            {
                _sourceState = value;
                SetCellText();
            }
        }

        private string _targetState;
        public string TargetState
        {
            get
            {
                return _targetState;
            }
            set
            {
                _targetState = value;
                SetCellText();
            }
        }

        private string _symbolRead;
        public string SymbolRead
        {
            get
            {
                return _symbolRead;
            }
            set
            {
                _symbolRead = value;
                SetCellText();
            }
        }

        private string _symbolWrite;
        public string SymbolWrite
        {
            get
            {
                return _symbolWrite;
            }
            set
            {
                _symbolWrite = value;
                SetCellText();
            }
        }

        private string _direction;
        public string Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                SetCellText();
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

        private bool _transitionDefined = false;
        public bool TransitionDefined
        {
            get { return _transitionDefined; }
            set
            {
                _transitionDefined = value;
                SetButtonEnable(value);
            }
        }

        private void SetButtonEnable(bool enabled)
        {
            addButton.IsEnabled = !enabled;
            editButton.IsEnabled = enabled;
            removeButton.IsEnabled = enabled;
        }

        private string SetDirection(string direction)
        {
            if (direction == "Right")
                return "→";
            else return direction == "Left" ? "←" : "•";
        }

        private void SetBackground()
        {
            TableCellGrid.Background = Highlight ? Brushes.Yellow : Brushes.White;
        }

        private void SetCellText()
        {
            TransitionText.Content = SymbolWrite + ", " + Direction + ", " + TargetState;
        }

        public TableCell()
        {
            InitializeComponent();
        }

        public TableCell(string sourceState, string targetState, string direction, string symbolWrite, string symbolRead, bool highlight)
        {
            InitializeComponent();
            SourceState = sourceState;
            TargetState = targetState;
            Direction = SetDirection(direction);
            SymbolWrite = symbolWrite;
            SymbolRead = symbolRead;
            Highlight = highlight;
            TransitionDefined = true;
        }

        private void add_transition_click(object sender, RoutedEventArgs e)
        {
            AddTransitionEvent?.Invoke();
        }

        private void edit_transition_click(object sender, RoutedEventArgs e)
        {
            EditTransitionEvent?.Invoke(SourceState, SymbolRead);
        }

        private void remove_transition_click(object sender, RoutedEventArgs e)
        {
            RemoveTransitionEvent?.Invoke(SourceState, SymbolRead);
        }
    }
}
