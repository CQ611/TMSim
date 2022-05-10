using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.Core
{
    public class TuringTransition
    {
        public enum Direction {Right, Left, Neutral}

        public TuringState Source { get; }
        public TuringState Target { get; }
        public List<char> SymbolsRead { get; }
        public List<char> SymbolsWrite { get; }
        
        public List<Direction> MoveDirections { get; }

        public TuringTransition(TuringState source, TuringState target,
            List<char> symbolsRead, List<char> symbolsWrite, List<Direction> dirs)
        {
            this.Source = source;
            this.Target = target;
            this.SymbolsRead = symbolsRead;
            this.SymbolsWrite = symbolsWrite;
            this.MoveDirections = dirs;
        }
        public bool CheckIfTransitionShouldBeActive(List<TuringTape> Tapes, TuringState CurrentState)
        {
            if (Source == CurrentState)
            {
                bool flag = true;
                for (int i = 0; i < SymbolsRead.Count() && flag; i++)
                {
                    if (SymbolsRead[i] != Tapes[i].GetCurrentSymbol()) flag = false;
                }
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
