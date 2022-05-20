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
        public string Comment { get; set; }
        public List<TuringTransition> IncomingTransitions { get; private set; }
        public List<TuringTransition> OutgoingTransitions { get; private set; }

        public bool IsStart = false;
        public bool IsAccepting = false;

        public TuringState(string identifier, string comment = "",
            bool isStart = false, bool isAccepting = false)
        {
            this.Identifier = identifier;
            this.Comment = comment;
            this.IsStart = isStart;
            this.IsAccepting = isAccepting;
            IncomingTransitions = new List<TuringTransition>();
            OutgoingTransitions = new List<TuringTransition>();
        }


        public void AssignIncomingTransition(TuringTransition tt)
        {
            IncomingTransitions.Add(tt);
        }
        public void AssignOutgoingTransition(TuringTransition tt)
        {
            OutgoingTransitions.Add(tt);
        }
    }
}
