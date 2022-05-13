using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.WPF
{
    public class NodeConnection
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public string Symbol { get; set; }

        public NodeConnection(string symbol, Node n1, Node n2) {
            Node1 = n1;
            Node2 = n2;
            Symbol = symbol;
        }
        public bool IsSelfReferencing()
        {
            return Node1 == Node2;
        }
    }
}
