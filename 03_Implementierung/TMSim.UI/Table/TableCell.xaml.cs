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
        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                LabelState.Content = _state;
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
                LabelSymbolWrite.Content = _symbolWrite;
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
                LabelMoveDirection.Content = _direction;
            }
        }

        public enum MoveDirection
        {
            Left,
            Right,
            Neutral
        }

        public void SetDirection(MoveDirection direction)
        {
            if (direction == MoveDirection.Left)
                Direction = "←";
            else if (direction == MoveDirection.Right)
                Direction = "→";
            else Direction = "•";
        }


        public TableCell()
        {
            InitializeComponent();
        }
    }
}
