using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMSim.Core;

namespace TMSim.UI
{
    [Serializable]
    public class Node : IEquatable<Node>
    {
        public TuringState State { get; set; }
        public string Identifier { get { return State.Identifier; } }
        public bool IsStart { get; set; } = false;
        public bool IsAccepting { get; set; } = false;
        public Point Position { get; set; } = new Point(50, 50);

        public Node(TuringState st, Point pos, bool start = false, bool accepting = false)
        {
            State = st;
            Position = pos;
            IsStart = start;
            IsAccepting = accepting;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Node);
        }

        public bool Equals(Node other)
        {
            return other.Identifier == this.Identifier;
        }

        public override int GetHashCode() => Identifier.GetHashCode();
    }
}
