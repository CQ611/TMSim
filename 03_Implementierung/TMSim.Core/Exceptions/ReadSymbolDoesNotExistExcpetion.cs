using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.Core.Exceptions
{
    public class ReadSymbolDoesNotExistException: Exception
    {
        public ReadSymbolDoesNotExistException(string message) : base(message)
        {
        }
    }
}
