using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMSim.Core
{
    public class Transformation5 : ITransformation
    {
        public TuringMachine Execute(TuringMachine tm, char ch = ' ')
        {
            if (tm.EndStates.Count == 1) return tm;
            if (tm.Tapes.Count != 1) {
                throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            }
            TuringMachine newTuringMachine = tm.GetCopy();
            foreach (TuringState oldEndstate in newTuringMachine.EndStates) {
                oldEndstate.IsAccepting = false;
            }
            String newEndStateName = "New Endstate";
            bool nameAlreadyExists;
            do
            {
                nameAlreadyExists = false;
                foreach (TuringState ts in newTuringMachine.States)
                {
                    if (ts.Identifier == newEndStateName)
                    {
                        newEndStateName = newEndStateName + "*";
                        nameAlreadyExists = true;
                        break;
                    }
                }
            } while (nameAlreadyExists);
            newTuringMachine.EndStates.Clear();
            TuringState newEndState = new TuringState(newEndStateName, "", false, true);
            newTuringMachine.AddState(newEndState);
            foreach(TuringState endState in tm.EndStates)
            {
                List<char> symbolsRead = new List<char>();
                foreach(TuringTransition transition in endState.OutgoingTransitions)
                {
                    symbolsRead.Add(transition.SymbolsRead[0]);
                }
                List<char> newTransitionSymbols = tm.TapeSymbols.Except(symbolsRead).ToList();
                foreach(char symbol in newTransitionSymbols)
                {
                    TuringTransition transition = new TuringTransition(
                        newTuringMachine.States.Find(item => item.Identifier == endState.Identifier),
                        newEndState, 
                        new List<char>() { symbol },
                        new List<char>() { symbol },
                        new List<TuringTransition.Direction>() { TuringTransition.Direction.Neutral }
                    );
                    newTuringMachine.AddTransition(transition);
                }
            }
            return newTuringMachine;
        }
        public bool IsExecutable(TuringMachine tm)
        {
            if (tm.EndStates.Count > 0) return true;
            return false;
        }
    }
}
