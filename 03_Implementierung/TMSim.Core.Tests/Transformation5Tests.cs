using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [DeploymentItem("res/example_import.tmsim", "res")]
    [DeploymentItem("res/example_import_one_endstate.tmsim", "res")]
    [DeploymentItem("res/example_contains_only_a_or_b.tmsim", "res")]
    [TestClass]
    public class Transformation5Tests
    {
        [TestMethod]
        public void IsExecutable_exampleImport_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            ITransformation transformation = new Transformation5();
            Assert.IsTrue(transformation.IsExecutable(turingMachine));
        }

        [TestMethod]
        public void IsExecutable_exampleImportWithoutEndStates_ReturnsFalse()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            turingMachine.EndStates.Clear();
            ITransformation transformation = new Transformation5();
            Assert.IsFalse(transformation.IsExecutable(turingMachine));
        }

        [TestMethod]
        public void Execute_exampleImportWithOneEndstate_ReturnsSameTuringMachine() 
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import_one_endstate.tmsim");
            ITransformation transformation = new Transformation5();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
            Assert.AreEqual(turingMachine, newTuringMachine);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Execute_exampleImportWithMoreThanOneTape_ThrowsNotImplementedException()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            ITransformation transformation = new Transformation5();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
        }

        [TestMethod]
        public void Execute_exampleImportOnlyAOrB_ReturnsNewTuringMachineWithOneEndState()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_contains_only_a_or_b.tmsim");
            ITransformation transfromation = new Transformation5();
            TuringMachine newTuringMachine = transfromation.Execute(turingMachine);
            Assert.IsTrue(newTuringMachine.EndStates.Count() == 1);
            Assert.IsTrue(newTuringMachine.EndStates[0].Identifier == "New Endstate");
            Assert.IsTrue(newTuringMachine.EndStates[0].IncomingTransitions.Count() == 2);
        }

        [TestMethod]
        public void Execute_exampleImportOnlyAOrB_AcceptsWordWhichOnlyContainsA()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_contains_only_a_or_b.tmsim");
            ITransformation transfromation = new Transformation5();
            TuringMachine newTuringMachine = transfromation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "aaaaa";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsTrue(newTuringMachine.CheckIsEndState());
        }

        [TestMethod]
        public void Execute_exampleImportOnlyAOrB_AcceptsWordWhichOnlyContainsB()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_contains_only_a_or_b.tmsim");
            ITransformation transfromation = new Transformation5();
            TuringMachine newTuringMachine = transfromation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "bbbbbbbbbbbbbbbbbbbbbb";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsTrue(newTuringMachine.CheckIsEndState());
        }

        [TestMethod]
        public void Execute_exampleImportOnlyAOrB_DoNotAcceptsWordWhichContainsAAndC()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_contains_only_a_or_b.tmsim");
            ITransformation transfromation = new Transformation5();
            TuringMachine newTuringMachine = transfromation.Execute(turingMachine);
            newTuringMachine.Tapes[0].Content = "aacaa";
            while (newTuringMachine.AdvanceState()) ;
            Assert.IsFalse(newTuringMachine.CheckIsEndState());
        }
    }
}