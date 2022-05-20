using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TMSim.Core;

namespace TMSim.UI
{
    public class TuringMachineModifier
    {
        private TuringMachine tm;
        private ViewModel vm;
        public TuringMachineModifier(ViewModel vm, ref TuringMachine tm)
        {
            this.vm = vm;
            this.tm = tm;
        }

        public void AddState()
        {
            AddStateDialog asd = new AddStateDialog($"q{tm.States.Count}");
            if (asd.ShowDialog() == true)
            {
                string identifier = asd.Identfier;
                bool isStart = asd.IsStart;
                bool isAccepting = asd.IsAccepting;
                string comment = "DEBUG: TODO: get comment from popup";
                //TODO: get comment from popup

                List<string> existingStates = new List<string>();
                tm.States.ForEach(ts => existingStates.Add(ts.Identifier));
                if (existingStates.Contains(identifier))
                {
                    MessageBox.Show($"State with identifier {identifier} already exists!",
                        "Warning!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                tm.AddState(new TuringState(identifier, comment, isStart, isAccepting));
                vm.OnTMChanged();
            }
        }

        public void RemoveState(string ident)
        {
            TuringState ts = tm.States.First(x => x.Identifier == ident);
            tm.RemoveState(ts);
            vm.OnTMChanged();
        }

        public void AddTransition(TuringState source = null, TuringState target = null)
        {
            List<char> symbolsRead = "ab".ToList<char>();
            List<char> symbolsWrite = "cd".ToList<char>();
            List<TuringTransition.Direction> direction = new List<TuringTransition.Direction> {
            TuringTransition.Direction.Right, TuringTransition.Direction.Left};
            //TODO: Get the above from new window
            // pay attention to source and target, may be null for Table, not for diagram
            MessageBox.Show("This should get values for a new Transition");

            tm.AddTransition(new TuringTransition(source, target, symbolsRead, symbolsWrite, direction));
            vm.OnTMChanged();
        }

        public void TransformT5()
        {
            vm.TM = new Transformation5().Execute(tm);
            vm.OnTMChanged();
        }
    }
}
