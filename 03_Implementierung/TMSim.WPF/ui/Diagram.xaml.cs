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

using System.Globalization;

namespace TMSim.WPF
{
    public partial class Diagram : UserControl
    {
        private Dictionary<string, Node> nodes; // (Identifier / Node object) pairs

        private List<NodeConnection> connections;
        public double NodeSize { get; set; } = 50f;
        public bool Animated { get; set; }

        private Node heldNode;
        
        private static readonly Brush bgBrush = Brushes.Black;
        private static readonly Pen bgPen = new Pen(bgBrush, 1);

        private static readonly Brush accentBrush = Brushes.Cyan;
        private static readonly Pen accentPen = new Pen(accentBrush, 1);
        public Diagram()
        {
            InitializeComponent();
            nodes = new Dictionary<string, Node>();
            connections = new List<NodeConnection>();
            //GenerateTestDiagram();
        }
        protected override async void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            dc.DrawRectangle(bgBrush, bgPen, new Rect(0, 0, ActualWidth, ActualHeight));

            if (heldNode != null)
            {
                heldNode.Position = Mouse.GetPosition(this);
                ConstrainToScreen(heldNode);
            }


            foreach (NodeConnection con in connections)
            {
                DrawNodeConnection(dc, con);
            }

            foreach (Node n in nodes.Values)
            {
                DrawNode(dc, n);
            }

            if (heldNode != null)
            {
                await Task.Delay(3);
                InvalidateVisual();
            }
        }

        private void DrawNode(DrawingContext dc, Node n)
        {
            dc.DrawEllipse(bgBrush, accentPen, n.Position, NodeSize / 2, NodeSize / 2);
            dc.DrawText(new FormattedText(n.Identifier,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Courier New"),
                NodeSize / 3, accentBrush, 1),
                OffsetPoint(n.Position, -NodeSize / 5, -NodeSize / 5));
        }

        private void DrawNodeConnection(DrawingContext dc, NodeConnection con)
        {
            if (con.IsSelfReferencing())
            {
                dc.DrawEllipse(Brushes.DarkBlue, accentPen,
                    OffsetPoint(con.Node1.Position, NodeSize / 2, NodeSize / 2),
                    NodeSize / 2, NodeSize / 2);
            }
            else
            {
                dc.DrawLine(accentPen, con.Node1.Position, con.Node2.Position);
            }
        }

        public void GenerateTestDiagram(int nodeCount = 10, int connectionCount = 15)
        {
            var rand = new Random();

            Node randNode()
            {
                return nodes[$"q{rand.Next(0, nodeCount - 1)}"];
            }

            nodes = new Dictionary<string, Node>();
            connections = new List<NodeConnection>();

            for (int i = 0; i < nodeCount; i++)
            {
                Node n = new Node($"q{i}", new Point(
                    rand.Next((int)NodeSize/2, (int)(ActualWidth - NodeSize/2)),
                    rand.Next((int)NodeSize / 2, (int)(ActualHeight - NodeSize / 2))),
                    start: i == 0);
                nodes.Add(n.Identifier, n);
            }
            for (int i = 0; i < connectionCount; i++)
            {
                connections.Add(new NodeConnection("t", randNode(), randNode()));
            }
            InvalidateVisual();
        }

        public async void ArrangeDiagram(int maxIterations = 5000, double stopForce = 0.2)
        {
            double repulsiveForce = 15000;
            double attractiveForce = 0.07;
            double relaxedSpringLength = NodeSize * 3;
            double gravityStrength = 2;

            int iterationCount = 0;
            double maxForce = 1000000;
            

            while (iterationCount < maxIterations && maxForce > stopForce)
            {
                iterationCount++;
                double coolingFactor = 1 - iterationCount / maxIterations;
                Vector[] resultingVectors = new Vector[nodes.Count];
                int ctr = 0;
                foreach (Node n in nodes.Values)
                {
                    Vector VectorSum = SumVectors(
                        new Vector[]{ GravityForceOn(n), SumOfAttractiveForcesOn(n), SumOfRepulsiveForcesOn(n)}
                        );
                    resultingVectors[ctr] = Vector.Multiply(coolingFactor, VectorSum);
                    ctr++;
                }

                maxForce = 0;
                ctr = 0;
                foreach (Node n in nodes.Values)
                {
                    maxForce = Math.Max(resultingVectors[ctr].Length, maxForce);
                    n.Position = Vector.Add(resultingVectors[ctr], n.Position);
                    ConstrainToScreen(n);
                    ctr++;
                }

                if (Animated)
                {
                    InvalidateVisual();
                    await Task.Delay(3);
                }
            }

            InvalidateVisual();

            Vector SumOfAttractiveForcesOn(Node n)
            {
                HashSet<Node> connectedNodes = new HashSet<Node>();
                foreach (NodeConnection con in connections)
                {
                    if (con.IsSelfReferencing()) continue;
                    else if (con.Node1 == n)
                    {
                        connectedNodes.Add(con.Node2);
                    }
                    else if (con.Node2 == n)
                    {
                        connectedNodes.Add(con.Node1);
                    }
                }

                List<Vector> attractiveVectors = new List<Vector>();
                foreach (Node other in connectedNodes)
                {
                    attractiveVectors.Add(Vector.Multiply(
                        attractiveForce * Math.Log(SubtractPositions(other, n).Length / relaxedSpringLength),
                        SubtractPositions(other, n)
                        ));
                }

                return SumVectors(attractiveVectors);
            }

            Vector SumOfRepulsiveForcesOn(Node n)
            {
                List<Node> otherNodes = new List<Node>();
                foreach (Node other in nodes.Values)
                {
                    if (other != n) otherNodes.Add(other);
                }

                List<Vector> repulsiveVectors = new List<Vector>();
                foreach (Node other in otherNodes)
                {
                    Vector difVector = SubtractPositions(n, other);
                    Vector directionVector = SubtractPositions(n, other);
                    directionVector.Normalize(); // yeah, a little ugly, but Normalize() returns void...
                    repulsiveVectors.Add(Vector.Multiply(
                        repulsiveForce / difVector.LengthSquared,
                        directionVector)
                        );
                }
                return SumVectors(repulsiveVectors);
            }

            Vector GravityForceOn(Node n)
            {
                Vector Center = new Vector(ActualWidth / 2, ActualHeight / 2);
                Vector dir = Vector.Subtract(Center, (Vector)n.Position);
                dir.Y = 2 * dir.Y;
                dir.Normalize();
                return dir * gravityStrength;
            }            
        }
        private Node ConstrainToScreen(Node n)
        {
            Point tmp = n.Position;
            double x = Math.Clamp(tmp.X, NodeSize / 2, ActualWidth - NodeSize / 2);
            double y = Math.Clamp(tmp.Y, NodeSize / 2, ActualHeight - NodeSize / 2);
            n.Position = new Point(x, y);
            return n;
        }

        private Point OffsetPoint(Point p, double offX, double offY)
        {
            return new Point(p.X + offX, p.Y + offY);
        }

        private Vector SubtractPositions(Node n1, Node n2)
        {
            return Vector.Subtract((Vector)n1.Position, (Vector)n2.Position);
        }

        private Vector SumVectors(IEnumerable<Vector> vectors)
        {
            Vector res = new Vector(0,0);
            foreach (Vector v in vectors)
            {
                res = Vector.Add(res, v);
            }
            return res;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector pos = (Vector)e.GetPosition((IInputElement)sender);
            //MessageBox.Show($"clicked at: {pos}");
            foreach (Node n in nodes.Values)
            {
                if (Vector.Subtract(pos, (Vector)n.Position).Length < NodeSize / 2)
                {
                    heldNode = n;
                    //MessageBox.Show($"picked up node {n.Identifier}");
                }
            }

            InvalidateVisual();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            heldNode = null;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            heldNode = null;
        }
    }
}
