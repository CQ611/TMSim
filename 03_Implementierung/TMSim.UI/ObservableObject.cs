
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace TMSim.UI
{
    [Serializable]
    abstract public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void ForcePropertyChangedEventHandler();
        public event ForcePropertyChangedEventHandler ForcePropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OnForcePropertyChanged() //no name, always invokes self
        {
            ForcePropertyChanged?.Invoke();
        }
    }
}
