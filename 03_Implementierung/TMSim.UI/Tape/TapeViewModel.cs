using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace TMSim.UI
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
                SetTimerInterval();
            }
        }

        public delegate void LoadTapeWord(string tapeWord);
        public event LoadTapeWord LoadTapeWordEvent;
        private void LoadTapeContent()
        {
            LoadTapeWordEvent?.Invoke(TM.Tapes[0].Content);
        }

        public delegate void UpdateTapeWord(string tapeWord);
        public event UpdateTapeWord UpdateTapeWordEvent;
        private void UpdateTapeContent()
        {
            DeleteTapeWordEvent?.Invoke(TM.BlankChar);
            UpdateTapeWordEvent?.Invoke(TM.Tapes[0].Content);
        }

        public delegate void DeleteTapeWord(char blank);
        public event DeleteTapeWord DeleteTapeWordEvent;
        private void DeleteTapeContent()
        {
            DeleteTapeWordEvent?.Invoke(TM.BlankChar);
        }

        public delegate void UpdateTape(int headIndex, double velocity);
        public event UpdateTape UpdateTapeEvent;
        private void UpdateTapeData()
        {
            UpdateTapeEvent?.Invoke(TM.Tapes[0].HeadIndex, TapeVelocity);
            DeleteTapeWordEvent?.Invoke(TM.BlankChar);
            UpdateTapeWordEvent?.Invoke(TM.Tapes[0].Content);
        }
    }
}
