using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace TMSim.WPF
{
    public partial class ViewModel : ObservableObject
    {
        public RelayCommand RightButton { get; set; }
        public RelayCommand LeftButton { get; set; }

        private double _tapeVelocity = 1000;
        public double TapeVelocity
        {
            get
            {
                return _tapeVelocity;
            }
            set
            {
                _tapeVelocity = value;
                OnPropertyChanged("TapeVelocity");
            }
        }

        public delegate void LeftMovement(double velocity);
        public event LeftMovement LeftMovementEvent;
        public void OnLeftButton()
        {
            LeftMovementEvent?.Invoke(TapeVelocity);
        }

        public delegate void RightMovement(double velocity);
        public event RightMovement RightMovementEvent;
        public void OnRightButton()
        {
            RightMovementEvent?.Invoke(TapeVelocity);
        }
    }
}
