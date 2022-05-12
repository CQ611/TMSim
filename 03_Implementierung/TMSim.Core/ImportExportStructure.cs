using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.Core
{
    class ImportExportStructure
    {
        public List<string> States { get; set; }
        public string InputAlphabet { get; set; }
        public string TapeAlphabet { get; set; }
        public List<Transition> Transitions { get; set; }
        public string StartState { get; set; }
        public char Blank { get; set; }
        public List<string> EndStates { get; set; }

        public class Transition
        {
            public string SourceState { get; set; }
            public string TargetState { get; set; }
            public List<char> SymbolsRead { get; set; }
            public List<char> SymbolsWrite { get; set; }
            public List<TuringTransition.Direction> MoveDirections { get; set; }
        }
    }
}
