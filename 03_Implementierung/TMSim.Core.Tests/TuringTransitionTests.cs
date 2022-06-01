using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMSim.Core.Exceptions;

namespace TMSim.Core.Tests
{
    [TestClass]
    public class TuringTransitionTests
    {
        [TestMethod]
        public void CheckIfTransitionShouldBeActive_ShouldReturnTrue()
        {
            TuringState source = new TuringState("");
            TuringState target = new TuringState("");
            TuringTape tape = new TuringTape("ka", '_');
            TuringTransition transition = new TuringTransition(source, target, new List<char>() {'k'}, new List<char>() {'k'}, new List<TuringTransition.Direction>() { TuringTransition.Direction.Left });

            Assert.IsTrue(transition.CheckIfTransitionShouldBeActive(new List<TuringTape>() {tape}, source));
        }
        [TestMethod]
        public void CheckIfTransitionShouldBeActive_ShouldReturnFalse()
        {
            TuringState source = new TuringState("");
            TuringState target = new TuringState("");
            TuringTape tape = new TuringTape("ka", '_');
            TuringTransition transition = new TuringTransition(source, target, new List<char>() { 'k' }, new List<char>() { 'k' }, new List<TuringTransition.Direction>() { TuringTransition.Direction.Left });

            Assert.IsFalse(transition.CheckIfTransitionShouldBeActive(new List<TuringTape>() { tape }, target));
        }

        [TestMethod]
        [ExpectedException(typeof(TransitionNumberOfTapesIsInconsistentException))]
        public void Constructor_throwsTransitionNumberOfTapesIsInconsistentException()
        {
            TuringState source = new TuringState("");
            TuringState target = new TuringState("");
            new TuringTransition(source, target, new List<char>() { 'k', 'a' }, new List<char>() { 'k' }, new List<TuringTransition.Direction>() { TuringTransition.Direction.Left });
        }
    }
}
