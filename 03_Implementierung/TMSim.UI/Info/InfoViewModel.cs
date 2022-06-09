using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        private Brush _infoBoxColor;
        public Brush InfoBoxColor
        {
            get
            {
                return _infoBoxColor;
            }
            set
            {
                _infoBoxColor = value;
                OnPropertyChanged(nameof(InfoBoxColor));
            }
        }

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

        public enum MessageIdentification
        {
            SimulationSuccess,
            SimulationFailure,
            SimulationIsRunning,
            SimulationIsPaused,
            SimulationIsStopped,
            SimulationSingleStep,
            InputWordWrittenOnTape,
            DefaultMessage
        }

        private MessageIdentification _messageID;
        public MessageIdentification MessageID
        {
            get
            {
                return _messageID;
            }
            set
            {
                _messageID = value;
                OnPropertyChanged(nameof(MessageID));
            }
        }

        private Brush GetBrushFromEnum(MessageIdentification messageIdentification)
        {
            if (messageIdentification == MessageIdentification.SimulationFailure)
                return Brushes.Red;
            else if (messageIdentification == MessageIdentification.SimulationSuccess)
                return Brushes.DarkGreen;
            else
                return Brushes.Gray;
        }

        private void UpdateInfo(MessageIdentification messageIdentification, string infoMessage)
        {
            MessageID = messageIdentification;
            InfoBoxColor = GetBrushFromEnum(messageIdentification);
            InfoMessage = infoMessage;
        }

        private void TranslateCurrentInfo()
        {
            switch(MessageID)
            {
                case MessageIdentification.SimulationSuccess:
                    UpdateInfo(MessageIdentification.SimulationSuccess, SimulationSuccessText);
                    break;
                case MessageIdentification.SimulationFailure:
                    UpdateInfo(MessageIdentification.SimulationFailure, SimulationFailureText);
                    break;
                case MessageIdentification.SimulationIsRunning:
                    UpdateInfo(MessageIdentification.SimulationIsRunning, SimulationIsRunningText);
                    break;
                case MessageIdentification.SimulationIsPaused:
                    UpdateInfo(MessageIdentification.SimulationIsPaused, SimulationIsPausedText);
                    break;
                case MessageIdentification.SimulationIsStopped:
                    UpdateInfo(MessageIdentification.SimulationIsStopped, SimulationIsStoppedText);
                    break;
                case MessageIdentification.SimulationSingleStep:
                    UpdateInfo(MessageIdentification.SimulationSingleStep, SimulationSingleStepText);
                    break;
                case MessageIdentification.DefaultMessage:
                    UpdateInfo(MessageIdentification.DefaultMessage, DefaultMessageText);
                    break;
                case MessageIdentification.InputWordWrittenOnTape:
                    UpdateInfo(MessageIdentification.InputWordWrittenOnTape, InputWordWrittenOnTapeText);
                    break;
            }
        }
    }

    internal struct NewStruct
    {
        public object Item1;
        public object Item2;

        public NewStruct(object item1, object item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct other &&
                   EqualityComparer<object>.Default.Equals(Item1, other.Item1) &&
                   EqualityComparer<object>.Default.Equals(Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Item1, Item2);
        }

        public void Deconstruct(out object item1, out object item2)
        {
            item1 = Item1;
            item2 = Item2;
        }

        public static implicit operator (object, object)(NewStruct value)
        {
            return (value.Item1, value.Item2);
        }

        public static implicit operator NewStruct((object, object) value)
        {
            return new NewStruct(value.Item1, value.Item2);
        }
    }
}
