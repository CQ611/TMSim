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

namespace TMSim.WPF
{
    public partial class Tape : UserControl
    {
        public List<char> BandContent { get; set; }

        private List<Label> TapeFields { get; set; }

        public Tape()
        {
            InitializeComponent();
            InitTapeFields();
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

        private int fieldUnderReadWriteHead = 12;

        public void WriteTapeWordToTape()
        {
            ClearTape();
            int i = 0;
            if (BandContent == null)
                return;
            foreach (var character in BandContent)
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

        public void ClearTape()
        {
            foreach (var tape in TapeFields)
            {
                tape.Content = "";
            }
        }

        private void ExampleLoadButton_Click(object sender, RoutedEventArgs e)
        {
            InitTapeFields();
            BandContent = new List<char>();
            foreach (var character in ExampleTapeWord.Text)
            {
                BandContent.Add(character);
            }
            fieldUnderReadWriteHead = 12;
            WriteTapeWordToTape();
        }

        private double actPos = 0;

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard();
            sb.Completed += MoveLeftCompleted;

            DoubleAnimation slide = new DoubleAnimation();
            slide.To = -34.0;
            slide.From = 0;
            actPos = -34.0;
            slide.Duration = new Duration(TimeSpan.FromMilliseconds(SliderExample.Value));

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

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard();
            sb.Completed += MoveRightCompleted;

            DoubleAnimation slide = new DoubleAnimation();
            slide.To = 34.0;
            slide.From = 0;
            actPos = 34.0;
            slide.Duration = new Duration(TimeSpan.FromMilliseconds(SliderExample.Value));

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
