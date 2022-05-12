using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.Core
{
    public class TuringState
    {
        public string Identifier { get; set; }

        public TuringState(string identifier)
        {
            this.Identifier = identifier;
        }
    }
}
