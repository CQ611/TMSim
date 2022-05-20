using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.UI
{
    public class DiagramData
    {
        public Dictionary<string, Node> Nodes { get; set; } // (Identifier / Node object) pairs
        public List<NodeConnection> Connections { get; set; }
        public double NodeSize { get; set; } = 50f;
        public double Width { get; set; }
        public double Height { get; set; }

        public System.Windows.Point AddNodePoint { get; set; }

        public DiagramData()
        {
            Nodes = new Dictionary<string, Node>();
            Connections = new List<NodeConnection>();
        }
    }
}
