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
                {TuringTransition.Direction.Right, "→" },
                {TuringTransition.Direction.Left, "←" },
                {TuringTransition.Direction.Neutral, "•" }
        };

        public TuringTransition Transition { get; set; }
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public List<NodeConnection> CollinearConnections { get; set; } = new List<NodeConnection>();
        public NodeConnection OpposedConnection { get; set; } = null;
        public string SymbolsWrite
        { get { return new string(Transition.SymbolsWrite.ToArray()); } }
        public string SymbolsRead
        { get { return new string(Transition.SymbolsRead.ToArray()); } }

        public string Directions
        {
            get
            {
                string res = "";
                foreach (TuringTransition.Direction dir in Transition.MoveDirections)
                {
                    res += dirTranslation[dir];
                }
                return res;
            }
        }

        public NodeConnection(TuringTransition tt, Node n1, Node n2) {
            Transition = tt;
            Node1 = n1;
            Node2 = n2;
            CollinearConnections.Add(this);
        }

        public bool IsSelfReferencing()
        {
            return Node1 == Node2;
        }

        public bool IsCollinear(NodeConnection other)
        {
            return other.Node1 == Node1 && other.Node2 == Node2;
        }

        public bool IsOpposed(NodeConnection other)
        {
            return other.Node1 == Node2 && other.Node2 == Node1;
        }
    }
}
