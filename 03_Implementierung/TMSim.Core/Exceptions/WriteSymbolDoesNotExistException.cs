using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.Core.Exceptions
{
    public class WriteSymbolDoesNotExistException: Exception
    {
        public WriteSymbolDoesNotExistException(string message) : base(message) 
        {

        }
    }
}
