using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TMSim.Core;

namespace TMSim.WPF
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
            if(asd.ShowDialog() == true)
            {
                string identifier = asd.Identfier;
                bool isStart = asd.IsStart;
                bool isAccepting = asd.IsAccepting;
                tm.AddState(identifier, isStart, isAccepting);
                vm.OnTMChanged();
            }
        }

        public void RemoveState(string ident)
        {
            TuringState ts = tm.States.Where(x => x.Identifier == ident).First();
            tm.RemoveState(ts);
            vm.OnTMChanged();
        }

        public void AddTransition(TuringState source = null, TuringState target = null)
        {
            List<char> symbolsRead = null;
            List<char> symbolsWrite = null;
            List<TuringTransition.Direction> direction = new List<TuringTransition.Direction>();
            //TODO: Get the above from new window
            // pay attention to source and target, may be null for Table, not for diagram
            MessageBox.Show("This should get values for a new Transition");

            tm.AddTransition(new TuringTransition(source, target, symbolsRead, symbolsWrite, direction));
            vm.OnTMChanged();
        }
    }
}
