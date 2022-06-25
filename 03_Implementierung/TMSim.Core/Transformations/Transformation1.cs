using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMSim.Core.Exceptions;

namespace TMSim.Core
{
    public class Transformation1 : ITransformation
    {
        private TuringMachine turingMachine;
        public TuringMachine Execute(TuringMachine tm, char c = ' ')
        {
            turingMachine = tm.GetCopy();

            if (tm.Tapes.Count != 1)
            {
                throw new NotImplementedException("This Transformation is only implemented for TuringMachines with one Tape");
            }
            

            if (IsPointingToStartState() || IsTransitionExitingAcceptingState())
            {
                Transform();
            }

            return turingMachine;
        }

        private void Transform()
        {
            TuringState alternativeStartState = new TuringState(GetNewIdentifier(), "", false, turingMachine.StartState.IsAccepting);
            turingMachine.AddState(alternativeStartState);
            foreach (TuringTransition transition in turingMachine.StartState.OutgoingTransitions) {
                TuringState target = transition.Target;
                if (transition.Target == turingMachine.StartState) { 
                    target = alternativeStartState; 
                }
                turingMachine.AddTransition(new TuringTransition(alternativeStartState, target, transition.SymbolsRead, transition.SymbolsWrite, transition.MoveDirections));
            }
            List<TuringTransition> incomingTransitions = new List<TuringTransition>(turingMachine.StartState.IncomingTransitions);
            if (incomingTransitions.Count() > 0)
            {
                foreach (TuringTransition transition in incomingTransitions)
                {
                    turingMachine.RemoveTransition(transition);
                    try
                    {
                        turingMachine.AddTransition(new TuringTransition(transition.Source, alternativeStartState, transition.SymbolsRead, transition.SymbolsWrite, transition.MoveDirections));
                    }
                    catch (TransitionAlreadyExistsException)
                    {
                        // happens when a transition leads from startState to startState
                    }
                }
            }
            else { // no transition leads back to startstate
                turingMachine.RemoveState(alternativeStartState);
            }
            List<TuringState> oldEndStates = new List<TuringState> (turingMachine.EndStates);
            TuringState newEndState = new TuringState(GetNewIdentifier(), "", false, true);
            turingMachine.AddState(newEndState);

            foreach (TuringState endState in oldEndStates)
            {
                List<char> symbolsRead = new List<char>();
                foreach (TuringTransition transition in endState.OutgoingTransitions)
                {
                    symbolsRead.Add(transition.SymbolsRead[0]);
                }
                List<char> newTransitionSymbols = turingMachine.TapeSymbols.Except(symbolsRead).ToList();
                foreach (char symbol in newTransitionSymbols)
                {
                    TuringTransition transition = new TuringTransition(
                        endState,
                        newEndState,
                        new List<char>() { symbol },
                        new List<char>() { symbol },
                        new List<TuringTransition.Direction>() { TuringTransition.Direction.Neutral }
                    );
                    turingMachine.AddTransition(transition);
                }
            }
            foreach (TuringState endState in oldEndStates) {
                // TODO: use EditState after bugFix
                endState.IsAccepting = false;
                turingMachine.EndStates.Remove(endState);
            }
        }


        private string GetNewIdentifier()
        {
            for (int i = 0; i < turingMachine.States.Count + 5; i++)
            {
                String newIdentifier = "q" + i; //finding Identifier that does not exist
                if (!turingMachine.States.Any(x => x.Identifier.Equals(newIdentifier)))
                {
                    return newIdentifier;
                }
            }

            Random rd = new Random();
            return "q" + rd.Next(1000, 1000000); //if somehow somthing went wrong
        }

        public bool IsPointingToStartState()
        {
            return turingMachine.StartState.IncomingTransitions.Any();
        }
        public bool IsTransitionExitingAcceptingState()
        {
            return turingMachine.Transitions.Where(x => x.Source.IsAccepting).Any(x => x.Source.OutgoingTransitions.Any());
        }
        public bool IsExecutable(TuringMachine tm)
        {
            if (tm.EndStates.Count > 0 && tm.StartState.IsStart) return true;
            return false;
        }
    }
}
