using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMSim.Core;

namespace TMSim.UI
{
    [Serializable]
    public class NodeConnection
    {
        Dictionary<TuringTransition.Direction, string> dirTranslation =
            new Dictionary<TuringTransition.Direction, string>()
        {
                {TuringTransition.Direction.Left, "←" },
                {TuringTransition.Direction.Right, "→" },
                {TuringTransition.Direction.Neutral, "•" }
        };

        public TuringTransition transition { get; set; }
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public string SymbolsWrite
        { 
            get { return new string(transition.SymbolsWrite.ToArray()); }
        }
        public string SymbolsRead
        {
            get { return new string(transition.SymbolsRead.ToArray()); }
        }
        public string Directions
        {
            get
            {
                string res = "";
                foreach (TuringTransition.Direction dir in transition.MoveDirections)
                {
                    res += dirTranslation[dir];
                }
                return res;
            }
        }

        public NodeConnection(TuringTransition tt, Node n1, Node n2) {
            transition = tt;
            Node1 = n1;
            Node2 = n2;
        }
        public bool IsSelfReferencing()
        {
            return Node1 == Node2;
        }
    }
}
