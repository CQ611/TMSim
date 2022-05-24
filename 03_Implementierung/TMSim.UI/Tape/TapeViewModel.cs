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
            UpdateTapeWordEvent?.Invoke(TM.Tapes[0].Content);
        }

        public delegate void DeleteTapeWord();
        public event DeleteTapeWord DeleteTapeWordEvent;
        private void DeleteTapeContent()
        {
            DeleteTapeWordEvent?.Invoke();
        }

        public delegate void UpdateTape(int headIndex, double velocity);
        public event UpdateTape UpdateTapeEvent;
        private void UpdateTapeData()
        {
            UpdateTapeEvent?.Invoke(TM.Tapes[0].HeadIndex, TapeVelocity);
            UpdateTapeWordEvent?.Invoke(TM.Tapes[0].Content);
        }
    }
}
