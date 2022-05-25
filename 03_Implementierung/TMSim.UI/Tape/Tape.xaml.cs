using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMSim.UI
{
    public partial class Tape : UserControl
    {
        public List<char> TapeContent { get; set; }

        private List<Label> TapeFields { get; set; }

        private int fieldUnderReadWriteHead = 12;
        private int headIndexOffset = 12;

        public Tape()
        {
            InitializeComponent();
            InitTapeFields();

            var vm = (ViewModel)DataContext;
            vm.LoadTapeWordEvent += Vm_LoadTapeWordEvent;
            vm.UpdateTapeWordEvent += Vm_UpdateTapeWordEvent;
            vm.DeleteTapeWordEvent += Vm_DeleteTapeWordEvent;
            vm.UpdateTapeEvent += Vm_UpdateTapeEvent;
        }

        private void Vm_UpdateTapeWordEvent(string tapeWord)
        {
            TapeContent = new List<char>();
            foreach (var character in tapeWord)
            {
                TapeContent.Add(character);
            }
            WriteTapeWordToTape();
        }

        private void Vm_UpdateTapeEvent(int headIndex, double velocity)
        {
            var newIndex = headIndex + headIndexOffset;
            if(newIndex < fieldUnderReadWriteHead)
            {
                RightMove(velocity);
            }
            else if (newIndex > fieldUnderReadWriteHead)
            {
                LeftMove(velocity);
            }
        }

        private void Vm_DeleteTapeWordEvent(char blank)
        {
            ClearTape(blank);
        }

        private void Vm_LoadTapeWordEvent(string tapeWord)
        {
            TapeContent = new List<char>();
            foreach (var character in tapeWord)
            {
                TapeContent.Add(character);
            }
            fieldUnderReadWriteHead = 12;
            WriteTapeWordToTape();
        }

        private void InitTapeFields()
        {
            TapeFields = new List<Label>();

            TapeFields.Add(tapePos0);
            TapeFields.Add(tapePos1);
            TapeFields.Add(tapePos2);
            TapeFields.Add(tapePos3);
            TapeFields.Add(tapePos4);
            TapeFields.Add(tapePos5);
            TapeFields.Add(tapePos6);
            TapeFields.Add(tapePos7);
            TapeFields.Add(tapePos8);
            TapeFields.Add(tapePos9);
            TapeFields.Add(tapePos10);
            TapeFields.Add(tapePos11);
            TapeFields.Add(tapePos12);
            TapeFields.Add(tapePos13);
            TapeFields.Add(tapePos14);
            TapeFields.Add(tapePos15);
            TapeFields.Add(tapePos16);
            TapeFields.Add(tapePos17);
            TapeFields.Add(tapePos18);
            TapeFields.Add(tapePos19);
            TapeFields.Add(tapePos20);
            TapeFields.Add(tapePos21);
            TapeFields.Add(tapePos22);
            TapeFields.Add(tapePos23);
            TapeFields.Add(tapePos24);
        }

        public void WriteTapeWordToTape()
        {
            int i = 0;
            if (TapeContent == null)
                return;
            foreach (var character in TapeContent)
            {
                if (i < TapeFields.Count() - fieldUnderReadWriteHead)
                {
                    if (fieldUnderReadWriteHead + i >= 0)
                    {
                        var label = TapeFields[fieldUnderReadWriteHead + i];
                        label.Content = character.ToString();
                    }
                }
                i++;
            }
        }

        public void ClearTape(char blank)
        {
            foreach (var tape in TapeFields)
            {
                tape.Content = blank.ToString();
            }
        }

        private double actPos = 0;

        private void LeftMove(double velocity)
        {
            Storyboard sb = new Storyboard();
            sb.Completed += MoveLeftCompleted;

            DoubleAnimation slide = new DoubleAnimation();
            slide.To = -34.0;
            slide.From = 0;
            actPos = -34.0;
            slide.Duration = new Duration(TimeSpan.FromMilliseconds(velocity));

            Storyboard.SetTarget(slide, TapeGrid);
            Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            sb.Children.Add(slide);
            sb.Begin();
        }

        private void MoveLeftCompleted(object sender, EventArgs e)
        {
            fieldUnderReadWriteHead--;
            ResetToDefault();
        }

        private void RightMove(double velocity)
        {
            Storyboard sb = new Storyboard();
            sb.Completed += MoveRightCompleted;

            DoubleAnimation slide = new DoubleAnimation();
            slide.To = 34.0;
            slide.From = 0;
            actPos = 34.0;
            slide.Duration = new Duration(TimeSpan.FromMilliseconds(velocity));

            Storyboard.SetTarget(slide, TapeGrid);
            Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            sb.Children.Add(slide);
            sb.Begin();
        }

        private void MoveRightCompleted(object sender, EventArgs e)
        {
            fieldUnderReadWriteHead++;
            ResetToDefault();
        }

        private void ResetToDefault()
        {
            WriteTapeWordToTape();

            Storyboard sb = new Storyboard();

            DoubleAnimation slide = new DoubleAnimation();
            slide.To = 0;
            slide.From = actPos;
            actPos = 0;
            slide.Duration = new Duration(TimeSpan.FromMilliseconds(0));

            Storyboard.SetTarget(slide, TapeGrid);
            Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            sb.Children.Add(slide);
            sb.Begin();
        }
    }
}
