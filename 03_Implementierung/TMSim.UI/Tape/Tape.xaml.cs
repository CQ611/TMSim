using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace TMSim.UI
{
    public partial class Tape : UserControl
    {
        public List<char> TapeContent { get; set; }

        private List<Label> TapeFields { get; set; }

        private char blankChar = ' ';
        private int fieldUnderReadWriteHead = 12;
        private int headIndexOffset = 12;

        private int leftrightchange = 12;

        public Tape()
        {
            InitializeComponent();
            InitTapeFields();

            var vm = (ViewModel)DataContext;
            vm.LoadTapeWordEvent += Vm_LoadTapeWordEvent;
            vm.UpdateTapeWordEvent += Vm_UpdateTapeWordEvent;
            vm.DeleteTapeWordEvent += Vm_DeleteTapeWordEvent;
            vm.UpdateTapeEvent += Vm_UpdateTapeEvent;
            vm.SetBlankEvent += Vm_SetBlankEvent;
            vm.UpdateAlphabetEvent += Vm_UpdateAlphabetEvent;

            Vm_UpdateAlphabetEvent(new List<char>(), new List<char>());
        }

        private void Vm_UpdateAlphabetEvent(List<char> tapeAlphabet, List<char> inputAlphabet)
        {
            //alphabet_tb.Inlines.Clear();
            //bool firstRun = true;

            //alphabet_tb.Inlines.Add(new Run("Alphabet={"));
            //foreach (var c in tapeAlphabet)
            //{
            //    if (!firstRun)
            //    {
            //        alphabet_tb.Inlines.Add(new Run(", "));
            //    }
            //    else firstRun = false;

            //    if (inputAlphabet.Contains(c))
            //        alphabet_tb.Inlines.Add(new Run(c.ToString()) { FontWeight = FontWeights.Bold });
            //    else
            //        alphabet_tb.Inlines.Add(new Run(c.ToString()));
            //}
            //alphabet_tb.Inlines.Add(new Run("}"));

            alphabet_tb.Inlines.Clear();
            bool firstRun = true;

            alphabet_tb.Inlines.Add(new Run("Alphabet={"));
            foreach (var c in inputAlphabet)
            {
                if (!firstRun)
                    alphabet_tb.Inlines.Add(new Run(", "));
                else firstRun = false;
                alphabet_tb.Inlines.Add(new Run(c.ToString()) { FontWeight = FontWeights.Bold });
            }

            foreach (var c in tapeAlphabet)
            {
                if (!inputAlphabet.Contains(c))
                {
                    if (!firstRun)
                    {
                        alphabet_tb.Inlines.Add(new Run(", "));
                    }
                    else firstRun = false;

                    alphabet_tb.Inlines.Add(new Run(c.ToString()));
                }
            }
            alphabet_tb.Inlines.Add(new Run("}"));
        }

        private void Vm_SetBlankEvent(char blank)
        {
            blankChar = blank;
        }

        private void Vm_UpdateTapeWordEvent(string tapeWord)
        {
            ClearTape();
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
            if (newIndex > leftrightchange)
            {
                LeftMove(velocity);
            }
            else if (newIndex < leftrightchange)
            {
                RightMove(velocity);
            }
        }

        private void Vm_DeleteTapeWordEvent()
        {
            ClearTape();
        }

        private void Vm_LoadTapeWordEvent(string tapeWord)
        {
            ClearTape();
            TapeContent = new List<char>();
            foreach (var character in tapeWord)
            {
                TapeContent.Add(character);
            }
            leftrightchange = 12;
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
            ClearTape();
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

        public void ClearTape()
        {
            foreach (var tape in TapeFields)
            {
                tape.Content = blankChar.ToString();
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
            leftrightchange++;
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
            leftrightchange--;
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
