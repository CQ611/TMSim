using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMSim.Core
{
    public class Transformation2 : ITransformation
    {
        public TuringMachine Execute(TuringMachine tm, Char ch = ' ')
        {
            if (tm.Tapes.Count != 1)
            {
                throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            }
            
            List<TuringTransition> neutralTransitions = new List<TuringTransition>(); 

            foreach (TuringTransition transition in tm.Transitions)
            {
                if(transition.MoveDirections[0] == TuringTransition.Direction.Neutral)
                {
                    neutralTransitions.Add(transition);
                }
            }

            if (neutralTransitions.Count == 0) return tm;

            TuringMachine newTuringMachine = tm.GetCopy();

            foreach (TuringTransition nt in neutralTransitions)
            {
                TuringState intermediateState = new TuringState(nt.Source.Identifier + "'", "intermediate state t2", false, false);
                newTuringMachine.States.Add(intermediateState);
                newTuringMachine.Transitions.Add(new TuringTransition(nt.Source, intermediateState, nt.SymbolsRead, nt.SymbolsWrite, new List<TuringTransition.Direction>() { TuringTransition.Direction.Right }));

                foreach (char tapeSymbol in newTuringMachine.TapeSymbols)
                {
                    newTuringMachine.Transitions.Add(new TuringTransition(intermediateState, nt.Target, new List<char>() { tapeSymbol }, new List<char>() { tapeSymbol }, new List<TuringTransition.Direction>() { TuringTransition.Direction.Left }));
                }
            }

            newTuringMachine.Transitions.RemoveAll(x => x.MoveDirections[0] == TuringTransition.Direction.Neutral);

            return newTuringMachine;
        }

        public bool IsExecutable(TuringMachine tm)
        {
            return true;
        }
    }
}
