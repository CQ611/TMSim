using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMSim.Core
{
    public class Transformation3 : ITransformation
    {
        public TuringMachine Execute(TuringMachine tm, Char newBlancToWrite)
        {
            TuringMachine turingMachine = tm;

            if (tm.Tapes.Count != 1)throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            
            turingMachine.Transitions.ForEach(x =>
            {
                if(x.SymbolsWrite.Contains(turingMachine.BlankChar))
                {
                    x.SymbolsWrite[0] = newBlancToWrite;
                }
            });
            turingMachine.AddSymbol(newBlancToWrite, false);
            return turingMachine;
        }


        public bool IsExecutable(TuringMachine tm)
        {
            throw new NotImplementedException();
        }
    }
}
