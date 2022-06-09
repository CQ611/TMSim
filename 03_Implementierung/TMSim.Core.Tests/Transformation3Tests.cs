using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [TestClass]
    public class Transformation3Tests
    {
        [TestMethod]
        public void Execute_import_transformation3_Swapes_Blanc_To_Write()
        {
            //prepare
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import_transformation3.tmsim");
            ITransformation transformation = new Transformation3();

            //before act
            Assert.IsTrue(turingMachine.Transitions.Any(x => x.SymbolsWrite[0].Equals('_')));

            //act
            TuringMachine newTuringMachine = transformation.Execute(turingMachine, '~');

            //assert
            Assert.IsFalse(newTuringMachine.Transitions.Any(x => x.SymbolsWrite[0].Equals('_')));
            Assert.IsTrue(newTuringMachine.Transitions.Any(x => x.SymbolsWrite[0].Equals('~')));
        }

        [TestMethod]
        public void Execute_example_contains_only_a_or_b_does_nothing()
        {
            //prepare
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_contains_only_a_or_b.tmsim");
            ITransformation transformation = new Transformation3();

            //act
            TuringMachine newTuringMachine = transformation.Execute(turingMachine, '~');

            //assert
            Assert.IsFalse(newTuringMachine.Transitions.Any(x => x.SymbolsWrite[0].Equals('~')));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Execute_exampleImportWithMoreThanOneTape_ThrowsNotImplementedException()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            ITransformation transformation = new Transformation3();
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);
        }

    }
}