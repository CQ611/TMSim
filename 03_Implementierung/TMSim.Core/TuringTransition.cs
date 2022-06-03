using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMSim.Core.Exceptions;

namespace TMSim.Core
{
    [Serializable]
    public class TuringTransition
    {
        public enum Direction {
            [EnumMember(Value = "Right")]
            Right,
            [EnumMember(Value = "Left")]
            Left,
            [EnumMember(Value = "Neutral")]
            Neutral
        }

        public TuringState Source { get; }
        public TuringState Target { get; }
        public List<char> SymbolsRead { get; }
        public List<char> SymbolsWrite { get; }

        public string Comment { get; set; }
        
        public List<Direction> MoveDirections { get; }

        public TuringTransition(TuringState source, TuringState target,
            List<char> symbolsRead, List<char> symbolsWrite, List<Direction> dirs, string comment="")
        {
            if (symbolsRead.Count() != symbolsWrite.Count() || symbolsRead.Count() != dirs.Count()) throw new TransitionNumberOfTapesIsInconsistentException();
            Source = source;
            Target = target;
            SymbolsRead = symbolsRead;
            SymbolsWrite = symbolsWrite;
            MoveDirections = dirs;
            Comment = comment;
        }


        public bool CheckIfTransitionShouldBeActive(List<TuringTape> Tapes, TuringState CurrentState)
        {
            if (Source == CurrentState)
            {
                bool flag = true;
                for (int i = 0; i < SymbolsRead.Count() && flag; i++)
                {
                    if (SymbolsRead[i] != Tapes[i].GetCurrentSymbol())
                    {
                        flag = false;
                    }
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
