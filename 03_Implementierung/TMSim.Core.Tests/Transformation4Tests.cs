using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [DeploymentItem("res/example_import.tmsim", "res")]
    [DeploymentItem("res/example_transformation4.tmsim", "res")]
    [DeploymentItem("res/example2_transformation4.tmsim", "res")]
    [DeploymentItem("res/example_with_neutral_transitions.tmsim", "res")]

    [TestClass]
    public class Transformation4Tests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Execute_exampleImportWithMoreThanOneTape_ThrowsNotImplementedException()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            ITransformation transformation = new Transformation4();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
        }

        [TestMethod]
        public void IsExecutable_exampleTransformation4_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation4.tmsim");
            ITransformation transformation = new Transformation4();
            Assert.IsTrue(transformation.IsExecutable(turingMachine));
        }

        [TestMethod]
        public void IsExecutable_exampleWithNeutralTransitions_ReturnsFalse()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_with_neutral_transitions.tmsim");
            ITransformation transformation = new Transformation4();
            Assert.IsFalse(transformation.IsExecutable(turingMachine));
        }

        [TestMethod]
        public void Execute_exampleTransformation4_StatesCountIs3()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation4.tmsim");
            ITransformation transformation = new Transformation4();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.States.Count == 3);
        }

        [TestMethod]
        public void Execute_exampleTransformation4_TransitionsCountIs2()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation4.tmsim");
            ITransformation transformation = new Transformation4();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.Transitions.Count == 2);
        }

        [TestMethod]
        public void Execute_example2Transformation4_StatesCountIs5()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example2_transformation4.tmsim");
            ITransformation transformation = new Transformation4();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.States.Count == 5);
        }

        [TestMethod]
        public void Execute_example2Transformation4_TransitionsCountIs7()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example2_transformation4.tmsim");
            ITransformation transformation = new Transformation4();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.Transitions.Count == 7);
        }
    }
}