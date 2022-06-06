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
    public class Node
    {
        public TuringState State { get; set; }
        public string Identifier { get { return State.Identifier; } }
        public bool IsStart { get { return State.IsStart; } }
        public bool IsAccepting { get { return State.IsAccepting; } }
        public Point Position { get; set; } = new Point(50, 50);
        public string Comment { get { return State.Comment; } }

        public Node(TuringState st, Point pos)
        {
            State = st;
            Position = pos;
        }
    }
}
