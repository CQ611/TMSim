using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.Core
{
    public class Transformation4 : ITransformation
    {
        public TuringMachine Execute(TuringMachine tm, char ch = ' ')
        {

            if (tm.Tapes.Count != 1)
            {
                throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            }

            TuringMachine newTuringMachine = tm.GetCopy();
            TuringMachine temporaryTuringMachine = tm.GetCopy();

            foreach (TuringState ts in tm.States)  //Anpassung für Transformationen die auf sich selber verweisen!!!
            {
                int directionRightCounter = 0;
                int directionLeftCounter = 0;

                foreach (TuringTransition itt in ts.IncomingTransitions)
                {
                    if (itt.MoveDirections[0] == TuringTransition.Direction.Right) directionRightCounter++;
                    if (itt.MoveDirections[0] == TuringTransition.Direction.Left) directionLeftCounter++;
                }

                if ((directionRightCounter >= 1) && (directionLeftCounter >= 1))
                {
                    TuringState currentState = temporaryTuringMachine.States.Find(x => x.Identifier == ts.Identifier);

                    if (directionRightCounter >= 1)
                    {
                        TuringState currentStateRight = new TuringState(ts.Identifier + "_right");
                        newTuringMachine.AddState(currentStateRight);

                        foreach (TuringTransition tt in currentState.IncomingTransitions)
                        {

                            if (tt.MoveDirections[0] == TuringTransition.Direction.Right)
                            {
                                TuringTransition transitionToRemove = newTuringMachine.Transitions.Find(x => (x.Source.Identifier == tt.Source.Identifier) && (x.SymbolsRead[0] == tt.SymbolsRead[0]));
                                newTuringMachine.RemoveTransition(transitionToRemove);
                                if (tt.Source.Identifier != tt.Target.Identifier)
                                {
                                    newTuringMachine.AddTransition(new TuringTransition(newTuringMachine.States.Find(x => x.Identifier == tt.Source.Identifier), currentStateRight, tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections, tt.Comment));

                                }
                                else
                                {
                                    newTuringMachine.AddTransition(new TuringTransition(currentStateRight, currentStateRight, tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections, tt.Comment));
                                }
                            }


                        }
                        foreach (TuringTransition tt in currentState.OutgoingTransitions)
                        {
                            if (tt.Source.Identifier != tt.Target.Identifier) newTuringMachine.AddTransition(new TuringTransition(currentStateRight, newTuringMachine.States.Find(x => x.Identifier == tt.Target.Identifier), tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections, tt.Comment));
                        }

                    }
                    if (directionLeftCounter >= 1)
                    {
                        TuringState currentStateLeft = new TuringState(ts.Identifier + "_left");
                        newTuringMachine.AddState(currentStateLeft);
                        foreach (TuringTransition tt in currentState.IncomingTransitions)
                        {
                            if (tt.MoveDirections[0] == TuringTransition.Direction.Left)
                            {
                                TuringTransition transitionToRemove = newTuringMachine.Transitions.Find(x => (x.Source.Identifier == tt.Source.Identifier) && (x.SymbolsRead[0] == tt.SymbolsRead[0]));
                                newTuringMachine.RemoveTransition(transitionToRemove);
                                if (tt.Source.Identifier != tt.Target.Identifier)
                                {
                                    newTuringMachine.AddTransition(new TuringTransition(newTuringMachine.States.Find(x => x.Identifier == tt.Source.Identifier), currentStateLeft, tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections, tt.Comment));
                                }
                                else
                                {
                                    newTuringMachine.AddTransition(new TuringTransition(currentStateLeft, currentStateLeft, tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections, tt.Comment));

                                }

                            }
                        }
                        foreach (TuringTransition tt in currentState.OutgoingTransitions)
                        {
                            if (tt.Source.Identifier != tt.Target.Identifier) newTuringMachine.AddTransition(new TuringTransition(currentStateLeft, newTuringMachine.States.Find(x => x.Identifier == tt.Target.Identifier), tt.SymbolsRead, tt.SymbolsWrite, tt.MoveDirections, tt.Comment));
                        }
                    }

                    newTuringMachine.RemoveState(newTuringMachine.States.Find(x => x.Identifier == currentState.Identifier)); //Damit sollten alle alten Transitionen gelöscht werden

                    temporaryTuringMachine.Reset();
                    temporaryTuringMachine = newTuringMachine.GetCopy();
                }
            }
            return newTuringMachine;
        }

        public bool IsExecutable(TuringMachine tm)
        {
            bool noNeutralTransition = true;
            foreach (TuringTransition tt in tm.Transitions)
            {
                if (tt.MoveDirections[0] == TuringTransition.Direction.Neutral) noNeutralTransition = false;
            }
            return noNeutralTransition;
        }
    }
}
