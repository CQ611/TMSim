using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [DeploymentItem("res/example_import.tmsim", "res")]
    [DeploymentItem("res/example_zero_neutral_transitions.tmsim", "res")]
    [DeploymentItem("res/example_with_neutral_transitions.tmsim", "res")]

    [TestClass]
    public class Transformation2Tests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Execute_exampleImportWithMoreThanOneTape_ThrowsNotImplementedException()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
        }

        [TestMethod]
        public void Execute_exampleImportWithZeroNeutralTransitions_ReturnsSameTuringMachine()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_zero_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.AreEqual(turingMachine, newTuringMachine);
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_ChangedStatesCountToThree_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.States.Count() == 3);
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_ChangedTransitionsCountToSeven_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.Transitions.Count() == 7);
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_NeutralTransintionsLeft_ReturnsFalse()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            foreach(TuringTransition transition in newTuringMachine.Transitions)
            {
                Assert.IsFalse(transition.MoveDirections[0] == TuringTransition.Direction.Neutral);
            }   
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_AcceptsWordWithOneA()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "a";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsTrue(newTuringMachine.CheckIsEndState());
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_AcceptsWordABC()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "abc";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsTrue(newTuringMachine.CheckIsEndState());
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_DoesNotAcceptWordBA()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "ba";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsFalse(newTuringMachine.CheckIsEndState());
        }

        [TestMethod]
        public void Execute_exampleImportWithNeutralTransitions_ChangedContentToB_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation2();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "a";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsTrue(newTuringMachine.Tapes[0].Content == "b_");
        }

    }
}