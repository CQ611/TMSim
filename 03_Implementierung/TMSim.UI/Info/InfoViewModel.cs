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
            DefaultMessage,
            T1Executed,
            T2Executed,
            T3Executed,
            T4Executed,
            T5Executed
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
                case MessageIdentification.T1Executed:
                    UpdateInfo(MessageIdentification.T1Executed, TransformationOneExecuted);
                    break;
                case MessageIdentification.T2Executed:
                    UpdateInfo(MessageIdentification.T2Executed, TransformationTwoExecuted);
                    break;
                case MessageIdentification.T3Executed:
                    UpdateInfo(MessageIdentification.T3Executed, TransformationThreeExecuted);
                    break;
                case MessageIdentification.T4Executed:
                    UpdateInfo(MessageIdentification.T4Executed, TransformationFourExecuted);
                    break;
                case MessageIdentification.T5Executed:
                    UpdateInfo(MessageIdentification.T5Executed, TransformationFiveExecuted);
                    break;
            }
        }
    }
}
