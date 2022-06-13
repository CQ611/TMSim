using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.Core
{
    public class Transformation4 : ITransformation
    {
        public TuringMachine Execute(TuringMachine tm, char ch = ' ')
        {
            if (tm.Tapes.Count != 1) throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            TuringMachine newTm = getPrepairedTuringMachine(tm);
            foreach (TuringTransition tt in tm.Transitions)
            {
                if (tt.Source == tm.StartState)
                {
                    if (tt.MoveDirections[0] == TuringTransition.Direction.Right)
                    {
                        TuringTransition newTt = new TuringTransition(newTm.StartState, newTm.States.Find(x => x.Identifier == tt.Target.Identifier + "_right"), tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections);
                        newTm.AddTransition(newTt);
                    }
                    else if (tt.MoveDirections[0] == TuringTransition.Direction.Left)
                    {
                        TuringTransition newTt = new TuringTransition(newTm.StartState, newTm.States.Find(x => x.Identifier == tt.Target.Identifier + "_left"), tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections);
                        newTm.AddTransition(newTt);
                    }
                }
                if (tt.MoveDirections[0] == TuringTransition.Direction.Right)
                {
                    TuringTransition newTtLeft = new TuringTransition(
                        newTm.States.Find(x => x.Identifier == tt.Source.Identifier + "_left"),
                        newTm.States.Find(x => x.Identifier == tt.Target.Identifier + "_right"),
                        tt.SymbolsRead,
                        tt.SymbolsWrite,
                        tt.MoveDirections
                        );
                    TuringTransition newTtRight = new TuringTransition(
                        newTm.States.Find(x => x.Identifier == tt.Source.Identifier + "_right"),
                        newTm.States.Find(x => x.Identifier == tt.Target.Identifier + "_right"),
                        tt.SymbolsRead,
                        tt.SymbolsWrite,
                        tt.MoveDirections
                        );
                    newTm.AddTransition(newTtRight);
                    newTm.AddTransition(newTtLeft);
                }
                else if (tt.MoveDirections[0] == TuringTransition.Direction.Left)
                {
                    TuringTransition newTtLeft = new TuringTransition(
                        newTm.States.Find(x => x.Identifier == tt.Source.Identifier + "_left"),
                        newTm.States.Find(x => x.Identifier == tt.Target.Identifier + "_left"),
                        tt.SymbolsRead,
                        tt.SymbolsWrite,
                        tt.MoveDirections
                        );
                    TuringTransition newTtRight = new TuringTransition(
                        newTm.States.Find(x => x.Identifier == tt.Source.Identifier + "_right"),
                        newTm.States.Find(x => x.Identifier == tt.Target.Identifier + "_left"),
                        tt.SymbolsRead,
                        tt.SymbolsWrite,
                        tt.MoveDirections
                        );
                    newTm.AddTransition(newTtRight);
                    newTm.AddTransition(newTtLeft);
                }
            }
            deleteUnusedStates(newTm);
            return newTm;
        }

        public bool IsExecutable(TuringMachine tm)
        {
            bool noNeutralTransition = true;
            foreach (TuringTransition tt in tm.Transitions)
            {
                if (tt.MoveDirections[0] == TuringTransition.Direction.Neutral) noNeutralTransition = false;
            }
            bool startStateNotEndStateAndNotNull = true;
            if (tm.EndStates.Contains(tm.StartState) || tm.StartState==null) startStateNotEndStateAndNotNull = false;

            return noNeutralTransition && startStateNotEndStateAndNotNull;
        }

        private void deleteUnusedStates(TuringMachine tm) {
            List<TuringState> unusedStates = new List<TuringState>();
            foreach (TuringState state in tm.States)
            {
                if (state.IncomingTransitions.Count == 0 && state != tm.StartState)
                {
                    unusedStates.Add(state);
                }
            }
            foreach (TuringState state in unusedStates)
            {
                tm.RemoveState(state);
            }
        }

        private TuringMachine getPrepairedTuringMachine(TuringMachine tm) {
            TuringMachine newTm = new TuringMachine();
            foreach (char c in tm.TapeSymbols)
            {
                if (tm.InputSymbols.Contains(c)) newTm.AddSymbol(c, true);
                else newTm.AddSymbol(c, false);
            }
            newTm.BlankChar = tm.BlankChar;
            foreach (TuringState state in tm.States)
            {
                TuringState rightState = new TuringState(state.Identifier + "_right", isAccepting: state.IsAccepting);
                TuringState leftState = new TuringState(state.Identifier + "_left", isAccepting: state.IsAccepting);
                newTm.AddState(rightState);
                newTm.AddState(leftState);
            }
            TuringState startState = new TuringState(tm.StartState.Identifier, isStart: true);
            newTm.AddState(startState);
            return newTm;
        }
    }
}