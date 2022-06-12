using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMSim.Core.Exceptions;

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
                try
                {
                    TuringState helperState = new TuringState(nt.Source.Identifier + "\'", "intermediate state t2", false, false);
                    newTuringMachine.AddState(helperState);
                    // Do not add states multiple times
                }
                catch (StateAlreadyExistsException) { };
                TuringState intermediateState = newTuringMachine.States.Find(x => x.Identifier == nt.Source.Identifier + "\'");
                TuringState sourceState = newTuringMachine.States.Find(x => x.Identifier == nt.Source.Identifier);
                TuringTransition ntInNewTm = newTuringMachine.Transitions.Find(
                    x => (
                        x.Source.Identifier == nt.Source.Identifier &&
                        x.SymbolsRead.SequenceEqual(nt.SymbolsRead)
                    )
                );
                try
                {
                    newTuringMachine.RemoveTransition(ntInNewTm);
                    // Maybe the transition was already removed
                }
                catch (TransitionDoesNotExistException) { }
                TuringTransition newTT = new TuringTransition(
                    sourceState,
                    intermediateState,
                    nt.SymbolsRead,
                    nt.SymbolsWrite,
                    new List<TuringTransition.Direction>() { TuringTransition.Direction.Right }
                );
                newTuringMachine.AddTransition(newTT);
                TuringState targetState = newTuringMachine.States.Find(x => x.Identifier == nt.Target.Identifier);
                foreach (char tapeSymbol in newTuringMachine.TapeSymbols)
                {
                    try
                    {
                        newTuringMachine.AddTransition(
                            new TuringTransition(
                                intermediateState,
                                targetState,
                                new List<char>() { tapeSymbol },
                                new List<char>() { tapeSymbol },
                                new List<TuringTransition.Direction>() { TuringTransition.Direction.Left }
                            )
                        );
                        // Do not add Transitions multiple Times
                    }
                    catch (TransitionAlreadyExistsException) { }
                }
            }
            return newTuringMachine;
        }

        public bool IsExecutable(TuringMachine tm)
        {
            return true;
        }
    }
}
