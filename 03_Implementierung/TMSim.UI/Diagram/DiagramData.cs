﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TMSim.UI
{
    [Serializable]
    public class DiagramData : ObservableObject
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