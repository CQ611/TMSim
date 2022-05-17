using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{
    [DeploymentItem("res/example_import.tmsim", "res")]
    [DeploymentItem("res/example_export.tmsim", "res")]
    [TestClass]
    public class Transformation5Tests
    {
        [TestMethod]
        public void IsExecutable_exampleImport_ReturnsTrue()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_import.tmsim");
            Transformation transformation = new Transformation5();
            Assert.IsTrue(transformation.IsExecutable(turingMachine));
        }
    }
}