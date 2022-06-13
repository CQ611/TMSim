using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                Transform(tm);
            }

            return turingMachine;
        }

        private void Transform(TuringMachine tm)
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
            foreach (TuringTransition transition in incomingTransitions) {
                turingMachine.RemoveTransition(transition);
                turingMachine.AddTransition(new TuringTransition(turingMachine.StartState, alternativeStartState, transition.SymbolsRead, transition.SymbolsWrite, transition.MoveDirections));
            }
            List<TuringState> oldEndStates = new List<TuringState> (turingMachine.EndStates);
            TuringState newEndState = new TuringState(GetNewIdentifier(), "", false, true);
            turingMachine.AddState(newEndState);

            foreach (TuringState endState in tm.EndStates)
            {
                List<char> symbolsRead = new List<char>();
                foreach (TuringTransition transition in endState.OutgoingTransitions)
                {
                    symbolsRead.Add(transition.SymbolsRead[0]);
                }
                List<char> newTransitionSymbols = tm.TapeSymbols.Except(symbolsRead).ToList();
                foreach (char symbol in newTransitionSymbols)
                {
                    TuringTransition transition = new TuringTransition(
                        turingMachine.States.Find(item => item.Identifier == endState.Identifier),
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
            try
            {
                Regex regex = new Regex("\\D");
                int maxIdent = turingMachine.Transitions
                    .Select(x => x.Source.Identifier)
                    .Select(x => regex.Replace(x.ToString(), ""))
                    .Select(x => int
                        .Parse(x.ToString()))
                        .ToList()
                        .Max();
                return "q" + (maxIdent + 2);
            }
            catch (Exception e)
            {
                //If something went wrong return qq0
                return "qq0";
            }
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
            return true;
        }
    }
}
