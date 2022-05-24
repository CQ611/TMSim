using System;

namespace TMSim.Core
{
    public class TransitionNotFoundException : Exception
    {
        public TransitionNotFoundException(string message) : base(message)
        {
        }
    }
}