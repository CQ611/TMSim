using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMSim.Core
{
    public class Transformation5 : Transformation
    {
        public TuringMachine Execute(TuringMachine tm)
        {
            if (tm.EndStates.Count == 1) return tm;
            //TuringState newEndState = new TuringState("endState");
            TuringMachine newTuringMachine = tm.GetCopy();
            newTuringMachine.EndStates.Clear();
            TuringState newEndState = new TuringState("new End State", "");
            newTuringMachine.AddState(newEndState, false, true);
            foreach(TuringState endState in tm.EndStates)
            {
                List<char> symbolsRead = new List<char>();
                foreach(TuringTransition transition in endState.OutgoingTransitions)
                {
                    symbolsRead.Add(transition.SymbolsRead[0]);
                }
                List<char> newTransitionSymbols = tm.TapeAlphabet.Symbols.Except(symbolsRead).ToList();
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
