using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Drawing;
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

using System.Globalization;
using System.ComponentModel;

namespace TMSim.WPF
{
    public partial class Diagram : UserControl
    {
        #region Dependency properties
        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Diagram)d).InvalidateVisual();
        }

        public static readonly DependencyProperty DDataProperty = DependencyProperty.Register(
            "DData", typeof(DiagramData), typeof(Diagram),
            new PropertyMetadata(null, OnDependencyPropertyChanged));


        public static readonly DependencyProperty TMModifierProperty = DependencyProperty.Register(
         "TMModifier", typeof(TuringMachineModifier), typeof(Diagram),
            new PropertyMetadata(null, OnDependencyPropertyChanged));

        public DiagramData DData
        {
            get { return (DiagramData)GetValue(DDataProperty); }
            set { SetValue(DDataProperty, value); }
        }

        public TuringMachineModifier TMModifier
        {
            get { return (TuringMachineModifier)GetValue(TMModifierProperty); }
            set { SetValue(TMModifierProperty, value); }
        }
        #endregion

        public bool Animated { get; set; }

        private Node heldNode;
        private Node rightClickedNode;

        Vector mouseClickpos;

        private static readonly Brush bgBrush = Brushes.White;
        private static readonly Pen bgPen = new Pen(bgBrush, 1);

        private static readonly Brush outlineBrush = Brushes.Black;
        private static readonly Pen outlinePen = new Pen(outlineBrush, 1);

        private static readonly Brush accentBrush = Brushes.LightGray;
        private static readonly Pen accentPen = new Pen(accentBrush, 1);
        public Diagram()
        {
            InitializeComponent();
            //GenerateTestDiagram();
            this.Loaded += new RoutedEventHandler(OnDiagramLoaded);
        }
        private void OnDiagramLoaded(object sender, RoutedEventArgs e)
        {
            DData.Width = ActualWidth;
            DData.Height = ActualHeight;
        }

        public void GenerateTestDiagram()
        {
            var rand = new Random();
            //DData = new DiagramData();
            TMModifier.AddState();


            InvalidateVisual();
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

            foreach (Node n in DData.Nodes.Values)
            {
                DrawNode(dc, n);
            }

            foreach (NodeConnection con in DData.Connections)
            {
                DrawNodeConnection(dc, con);
            }

            if (heldNode != null)
            {
                await Task.Delay(3);
                InvalidateVisual();
            }
        }

        private void DrawNode(DrawingContext dc, Node n)
        {
            dc.DrawEllipse(bgBrush, outlinePen, n.Position, DData.NodeSize / 2, DData.NodeSize / 2);
            dc.DrawText(new FormattedText(n.Identifier,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Courier New"),
                DData.NodeSize / 3, outlineBrush, 1),
                OffsetPoint(n.Position, -DData.NodeSize / 5, -DData.NodeSize / 5));
        }

        private void DrawNodeConnection(DrawingContext dc, NodeConnection con)
        {
            if (con.IsSelfReferencing())
            {
                dc.DrawEllipse(Brushes.DarkBlue, outlinePen,
                    OffsetPoint(con.Node1.Position, DData.NodeSize / 2, DData.NodeSize / 2),
                    DData.NodeSize / 2, DData.NodeSize / 2);
            }
            else
            {
                dc.DrawLine(outlinePen,
                    PointOnBorderTowards(con.Node1, con.Node2),
                    PointOnBorderTowards(con.Node2, con.Node1)
                    );
                DrawArrwohead(
                    dc,
                    PointOnBorderTowards(con.Node2, con.Node1),
                    con.Node1.Position - con.Node2.Position
                    );
                DrawConnectionText(dc, con, CenterPoint(con));
            }
        }

        private void DrawConnectionText(DrawingContext dc, NodeConnection con, Point textCenter)
        {
            string text = con.SymbolsRead + "|" + con.Directions + "|" + con.SymbolsWrite;

            //System.Drawing.MeasureString()
            FormattedText fText = new FormattedText(text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Courier New"),
                DData.NodeSize / 3, outlineBrush, 1);

            dc.PushTransform(new TranslateTransform(
                textCenter.X - fText.Width / 2, 
                textCenter.Y - fText.Height / 2));
            dc.DrawRectangle(accentBrush, null, new Rect(
                new Size(fText.Width, fText.Height)
                ));
            dc.DrawText(fText, new Point(0,0));

            dc.Pop();
        }

        private Point CenterPoint(NodeConnection con)
        {
            return (Point)(((Vector)con.Node1.Position + (Vector)con.Node2.Position) / 2);
        }

        private Point PointOnBorderTowards(Node source, Node target)
        {
            Vector direction =  target.Position - source.Position;
            direction.Normalize();
            return source.Position + direction * DData.NodeSize/2;
        }

        private void DrawArrwohead(DrawingContext dc, Point tip, Vector direction)
        {
            direction.Normalize();
            Point corner1 = tip + direction * DData.NodeSize / 6 +
                PerpendicularVector(direction) * DData.NodeSize / 12;
            Point corner2 = tip + direction * DData.NodeSize / 6 -
                PerpendicularVector(direction) * DData.NodeSize / 12;

            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext ctx = sg.Open())
            {
                ctx.BeginFigure(tip, true, true);
                ctx.LineTo(corner1, false, true);
                ctx.LineTo(corner2, false, true);
            }
            sg.Freeze();
            dc.DrawGeometry(outlineBrush, outlinePen, sg);
        }

        private Vector PerpendicularVector(Vector v)
        {
            return new Vector(-v.Y, v.X);
        }

        public async void ArrangeDiagram(int maxIterations = 5000, double stopForce = 0.2)
        {
            double repulsiveForce = 15000;
            double attractiveForce = 0.07;
            double relaxedSpringLength = DData.NodeSize * 3;
            double gravityStrength = 2;

            int iterationCount = 0;
            double maxForce = 1000000;


            while (iterationCount < maxIterations && maxForce > stopForce)
            {
                iterationCount++;
                double coolingFactor = 1 - iterationCount / maxIterations;
                Vector[] resultingVectors = new Vector[DData.Nodes.Count];
                int ctr = 0;
                foreach (Node n in DData.Nodes.Values)
                {
                    Vector VectorSum = SumVectors(
                        new Vector[]{ GravityForceOn(n), SumOfAttractiveForcesOn(n), SumOfRepulsiveForcesOn(n)}
                        );
                    resultingVectors[ctr] = Vector.Multiply(coolingFactor, VectorSum);
                    ctr++;
                }

                maxForce = 0;
                ctr = 0;
                foreach (Node n in DData.Nodes.Values)
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
                foreach (NodeConnection con in DData.Connections)
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
                foreach (Node other in DData.Nodes.Values)
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
            double x = Math.Clamp(tmp.X, DData.NodeSize / 2, ActualWidth - DData.NodeSize / 2);
            double y = Math.Clamp(tmp.Y, DData.NodeSize / 2, ActualHeight - DData.NodeSize / 2);
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
            mouseClickpos = (Vector)e.GetPosition((IInputElement)sender);
            //MessageBox.Show($"clicked at: {pos}");
            foreach (Node n in DData.Nodes.Values)
            {
                if (Vector.Subtract(mouseClickpos, (Vector)n.Position).Length < DData.NodeSize / 2)
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

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseClickpos = (Vector)e.GetPosition((IInputElement)sender);
            bool found = false;
            foreach (Node n in DData.Nodes.Values)
            {
                if (Vector.Subtract(mouseClickpos, (Vector)n.Position).Length < DData.NodeSize / 2)
                {
                    rightClickedNode = n;
                    found = true;
                    //MessageBox.Show($"picked up node {n.Identifier}");
                    break;
                }
            } 
            if(!found)
            {
                rightClickedNode = null;
            }

            InvalidateVisual();
        }

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Node nodeReleasedOver = null;
            //this vvv is intentially not mouseClickpos
            Vector pos = (Vector)e.GetPosition((IInputElement)sender);
            foreach (Node n in DData.Nodes.Values)
            {
                if (Vector.Subtract(pos, (Vector)n.Position).Length < DData.NodeSize / 2)
                {
                    nodeReleasedOver = n;
                    //MessageBox.Show($"picked up node {n.Identifier}");
                }
            }
            if (nodeReleasedOver == rightClickedNode)
            {
                //Open context menu
                // with the option of creating a connection (also) to it self
            }
            else if (nodeReleasedOver != null && rightClickedNode != null)
            {
                //create Connection
                TMModifier.AddTransition(rightClickedNode.State, nodeReleasedOver.State);
                InvalidateVisual();
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            heldNode = null;
        }

        private void add_state_btn_Click(object sender, RoutedEventArgs e)
        {
            DData.AddNodePoint = (Point)mouseClickpos;
            TMModifier.AddState();
            InvalidateVisual();
        }

        private void remove_state_btn_Click(object sender, RoutedEventArgs e)
        {
            if(rightClickedNode != null)
            {
                TMModifier.RemoveState(rightClickedNode.Identifier);
                InvalidateVisual();
            }
        }

        private void arrange_btn_Click(object sender, RoutedEventArgs e)
        {
            ArrangeDiagram();
            InvalidateVisual();
        }
    }
}
