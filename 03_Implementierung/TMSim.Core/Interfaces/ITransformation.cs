using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.Core
{
    public interface ITransformation
    {
        public bool IsExecutable(TuringMachine tm);
        public TuringMachine Execute(TuringMachine tm, Char newBlankToWrite = ' ');
    }
}
