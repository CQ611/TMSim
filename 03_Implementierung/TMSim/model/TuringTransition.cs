using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.model
{
    class TuringTransition
    {
        public enum Direction {Right, Left, Neutral}

        public TuringState Source { get; }
        public TuringState Target { get; }
        public char SymbolRead { get; }
        public char SymbolWrite {get; }
        
        public Direction MoveDirection { get; }

        public TuringTransition(TuringState source, TuringState target,
            char symbolRead, char symbolWrite, Direction dir)
        {
            this.Source = source;
            this.Target = target;
            this.SymbolRead = symbolRead;
            this.SymbolWrite = symbolWrite;
            this.MoveDirection = dir;
        }
    }
}
