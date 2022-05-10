using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.model
{
    class TuringTransition
    {
        public enum Direction {Right, Left}

        public TuringState Source { get; }
        public TuringState Target { get; }
        public char Symbol { get; }
        
        public Direction MoveDirection { get; }

        public TuringTransition(TuringState source, TuringState target,
            char symbol, Direction dir)
        {
            this.Source = source;
            this.Target = target;
            this.Symbol = symbol;
            this.MoveDirection = dir;
        }
    }
}
