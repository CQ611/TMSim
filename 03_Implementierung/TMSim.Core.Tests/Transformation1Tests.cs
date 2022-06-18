using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMSim.Core.Tests
{

    //[DeploymentItem("res/example_TranformationEins.tmsim", "res")]

    [TestClass]
    public class Transformation1Tests
    {
        [TestMethod]
        public void HasCorrectNumberOfTransitionsAndStates()
        {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation_eins.tmsim");
            ITransformation transformation = new Transformation1();

            Assert.AreEqual(3, turingMachine.States.Count);
            Assert.AreEqual(4, turingMachine.Transitions.Count);

            TuringMachine newTransformatedTuringMachine = transformation.Execute(turingMachine);

            
            Assert.AreEqual(5, newTransformatedTuringMachine.States.Count);
            Assert.AreEqual(40, newTransformatedTuringMachine.Transitions.Count);
        }


        [TestMethod]
        public void StartStateHasNoIncomingTransitions_IsTrue()
        {
            //init
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation_eins.tmsim");
            ITransformation transformation = new Transformation1();

            //act
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);

            //assert
            Assert.AreEqual(0, newTuringMachine.StartState.IncomingTransitions.Count);
        }

        [TestMethod]
        public void StartStateHasThreeOutgoingTransitions()
        {
            //init
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation_eins.tmsim");
            ITransformation transformation = new Transformation1();

            //act
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);

            //assert
            Assert.AreEqual(13, newTuringMachine.Transitions.First(x => x.Source.IsStart).Source.OutgoingTransitions.Count);
            Assert.AreEqual(13, newTuringMachine.StartState.OutgoingTransitions.Count);
        }

        [TestMethod]
        public void NewStateHas4IncomingTransitionsAndThirteenOutgoingTransitions()
        {
            //init
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation_eins.tmsim");
            ITransformation transformation = new Transformation1();

            //act
            TuringMachine newTuringMachine = transformation.Execute(turingMachine);

            TuringState turingState = newTuringMachine.States.First(x => x.Identifier.Equals("q3"));

            //assert
            Assert.AreEqual(4, turingState.IncomingTransitions.Count);
            Assert.AreEqual(13, turingState.OutgoingTransitions.Count);
        }


        [TestMethod]
        public void TransformationOneDoesNothingToasdf_to_tobi()
        {
            //init
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_asdf_to_tobi.tmsim");
            ITransformation transformation = new Transformation1();

            //before act
            Assert.AreEqual(7, turingMachine.States.Count);
            Assert.AreEqual(6, turingMachine.Transitions.Count);

            //act
            TuringMachine newTransformatedTuringMachine = transformation.Execute(turingMachine);

            //assert
            Assert.AreEqual(7, newTransformatedTuringMachine.States.Count);
            Assert.AreEqual(6, newTransformatedTuringMachine.Transitions.Count);
        }

        [TestMethod]
        public void AlternativeStartState_isRemovedWhenNoTransitionLeadsToStartState() {
            TuringMachine turingMachine = new TuringMachine();
            turingMachine.ImportFromTextFile(@"res\example_transformation_1_no_transition_to_start_state.tmsim");
            ITransformation transformation = new Transformation1();

            //before act
            Assert.AreEqual(2, turingMachine.States.Count);
            Assert.AreEqual(2, turingMachine.Transitions.Count);

            //act
            TuringMachine newTransformatedTuringMachine = transformation.Execute(turingMachine);

            //assert
            Assert.AreEqual(3, newTransformatedTuringMachine.States.Count);
            Assert.AreEqual(4, newTransformatedTuringMachine.Transitions.Count);
        }
    }
}