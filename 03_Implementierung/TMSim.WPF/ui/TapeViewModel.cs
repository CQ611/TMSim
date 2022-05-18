using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TMSim.WPF
{
    public partial class ViewModel : ObservableObject
    {
        public RelayCommand RightButton { get; set; }
        public RelayCommand LeftButton { get; set; }

        private double _tapeVelocity;
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

        private double _xTransformGrid;
        public double XTransformGrid
        {
            get
            {
                return _xTransformGrid;
            }
            set
            {
                _xTransformGrid = value;
                OnPropertyChanged("XTransformGrid");
            }
        }

        private Grid _tapeGrid;
        public Grid TapeGrid 
        { 
            get
            {
                return _tapeGrid;
            }
            set
            {
                _tapeGrid = value;
                OnPropertyChanged(nameof(TapeGrid));
            }
        }

        private double actPos = 0;

        public void OnLeftButton()
        {
            //Storyboard sb = new Storyboard();
            //sb.Completed += MoveLeftCompleted;

            //DoubleAnimation slide = new DoubleAnimation();
            //slide.To = -34.0;
            //slide.From = 0;
            //actPos = -34.0;
            //slide.Duration = new Duration(TimeSpan.FromMilliseconds(TapeVelocity));

            //Storyboard.SetTarget(slide, TapeGrid);
            //Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            //sb.Children.Add(slide);
            //sb.Begin();
            XTransformGrid = -304;
        }

        public void OnRightButton()
        {
            //Storyboard sb = new Storyboard();
            //sb.Completed += MoveRightCompleted;

            //DoubleAnimation slide = new DoubleAnimation();
            //slide.To = 34.0;
            //slide.From = 0;
            //actPos = 34.0;
            //slide.Duration = new Duration(TimeSpan.FromMilliseconds(TapeVelocity));

            //Storyboard.SetTarget(slide, TapeGrid);
            //Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            //sb.Children.Add(slide);
            //sb.Begin();

            XTransformGrid = 304;
        }


        private void MoveLeftCompleted(object sender, EventArgs e)
        {
            //fieldUnderReadWriteHead--;
            ResetToDefault();
        }


        private void MoveRightCompleted(object sender, EventArgs e)
        {
            //fieldUnderReadWriteHead++;
            ResetToDefault();
        }

        private void ResetToDefault()
        {
            //WriteTapeWordToTape();

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
