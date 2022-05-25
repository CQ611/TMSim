using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        private string _infoMessage = String.Empty;
        public string InfoMessage
        {
            get
            {
                return _infoMessage;
            }
            set
            {
                _infoMessage = value;
                OnPropertyChanged(nameof(InfoMessage));
            }
        }

        private enum MessageColor
        {
            Red,
            DarkGreen

        }

        private Brush GetBrushFromEnum(MessageColor messageColor)
        {
            if (messageColor == MessageColor.Red)
                return Brushes.Red;
            else if (messageColor == MessageColor.DarkGreen)
                return Brushes.DarkGreen;
            else
                return Brushes.Black;
        }

        private Brush _infoMessageColor;
        public Brush InfoMessageColor
        {
            get
            {
                return _infoMessageColor;
            }
            set
            {
                _infoMessageColor = value;
                OnPropertyChanged(nameof(InfoMessageColor));
            }
        }
    }
}
