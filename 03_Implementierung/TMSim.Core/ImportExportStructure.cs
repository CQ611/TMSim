using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;

namespace TMSim.Core
{
    class ImportExportStructure
    {
        public List<State> States { get; set; }
        public class State 
        {
            public string Identifier { get; set; }
            public string Comment { get; set; }
        }
        public string InputAlphabet { get; set; }
        public string TapeAlphabet { get; set; }
        public List<Transition> Transitions { get; set; }
        public string StartState { get; set; }
        public char Blank { get; set; }
        public List<string> EndStates { get; set; }

        public class Transition
        {
            public string SourceState { get; set; }
            public string TargetState { get; set; }
            public List<char> SymbolsRead { get; set; }
            public List<char> SymbolsWrite { get; set; }
            [JsonProperty(ItemConverterType =typeof(StringEnumConverter))]
            public List<TuringTransition.Direction> MoveDirections { get; set; }
            public string Comment { get; set; }
        }

        public ImportExportStructure() { }

        public ImportExportStructure(TuringMachine turingMachine) {
            States = new List<State>();
            Transitions = new List<Transition>();
            EndStates = new List<string>();
            foreach (TuringState state in turingMachine.States) {
                State internalState = new State();
                internalState.Identifier = state.Identifier;
                internalState.Comment = state.Comment;
                States.Add(internalState);
            }
            InputAlphabet = new string(turingMachine.InputSymbols.ToArray());
            TapeAlphabet = new string(turingMachine.TapeSymbols.ToArray());
            foreach (TuringTransition turingTransition in turingMachine.Transitions) {
                Transition transition = new Transition();
                transition.SourceState = turingTransition.Source.Identifier;
                transition.TargetState = turingTransition.Target.Identifier;
                transition.SymbolsRead = turingTransition.SymbolsRead;
                transition.SymbolsWrite = turingTransition.SymbolsWrite;
                transition.MoveDirections = turingTransition.MoveDirections;
                transition.Comment = turingTransition.Comment;
                Transitions.Add(transition);
            }
            if (turingMachine.StartState != null)
            {
                StartState = turingMachine.StartState.Identifier;
            }
            Blank = turingMachine.BlankChar;
            foreach (TuringState endState in turingMachine.EndStates) 
            {
                EndStates.Add(endState.Identifier);
            }
        }
    }
}
