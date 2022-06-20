using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMSim.Core
{
    public class Transformation3 : ITransformation
    {
        private Char newBlank;
        private TuringMachine turingMachine;
        public TuringMachine Execute(TuringMachine tm, char newBlancToWrite)
        {
            turingMachine = tm;
            newBlank = newBlancToWrite;

            if (!IsInvalidTransformation())
            {
                turingMachine.AddSymbol(newBlancToWrite, false);

                turingMachine.Transitions.ForEach(x =>
                {
                    if (x.SymbolsWrite.Contains(turingMachine.BlankChar))
                    {
                        x.SymbolsWrite[0] = newBlancToWrite;
                    }
                });
                AddBlankTransitions(turingMachine, newBlancToWrite);
            }
            return turingMachine;
        }

        private bool IsInvalidTransformation()
        {
            if (turingMachine.Tapes.Count != 1) throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            return turingMachine.TapeSymbols.Contains(newBlank);
        }

        private void AddBlankTransitions(TuringMachine turingMachine, Char newBlancToWrite)
        {
            List<char> newList = new List<char>();
            newList.Add(newBlancToWrite);
            List<TuringTransition> TuringTransitionsWithBlancs = new List<TuringTransition>(turingMachine.Transitions.Where(x => x.SymbolsRead[0].Equals(turingMachine.BlankChar) || x.SymbolsRead[0].Equals(newBlancToWrite))); 

            foreach (TuringTransition turingTransition in TuringTransitionsWithBlancs)
            {
                turingMachine.AddTransition(new TuringTransition(turingTransition.Source, 
                    turingTransition.Target,
                    newList,
                    turingTransition.SymbolsWrite, 
                    turingTransition.MoveDirections));
            }
        }


        public bool IsExecutable(TuringMachine tm)
        {
            throw new NotImplementedException();
        }
    }
}
