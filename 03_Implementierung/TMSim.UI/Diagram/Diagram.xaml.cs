using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using System.Globalization;
using System.ComponentModel;

namespace TMSim.UI
{
    public partial class Diagram : UserControl
    {
        #region Dependency properties
        public static DependencyProperty DDataProperty = DependencyProperty.Register(
            "DData", typeof(DiagramData), typeof(Diagram),
            new PropertyMetadata(null, OnDependencyPropertyChanged));

        public static DependencyProperty VMProperty = DependencyProperty.Register(
            "VM", typeof(ViewModel), typeof(Diagram),
            new PropertyMetadata(null, OnDependencyPropertyChanged));

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Diagram)d).PropertyChangedCallback();
        }

        private void PropertyChangedCallback()
        {
            if (DData != null) {if (DData.ArangeFlag) ArangeDiagram(); DData.ArangeFlag = false; }
            InvalidateVisual();
        }
        #endregion


        public DiagramData DData
        {
            get { return (DiagramData)GetValue(DDataProperty); }
            set { SetValue(DDataProperty, value); }
        }
        public ViewModel VM
        {
            get { return (ViewModel)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }

        private Node heldNode;
        private Node rightClickedNode;
        private NodeConnection rightClickedConnection;

        Vector mouseClickpos;

        private static readonly Brush bgBrush = Brushes.White;
        private static readonly Pen bgPen = new Pen(bgBrush, 1);

        private static readonly Brush outlineBrush = Brushes.Black;
        private static readonly Pen outlinePen = new Pen(outlineBrush, 1);

        private static readonly Brush accentBrush = Brushes.LightGray;
        private static readonly Pen accentPen = new Pen(accentBrush, 1);

        private static readonly Brush highlightBrush = Brushes.Yellow;
        private static readonly Pen highlightPen = new Pen(highlightBrush, 1);

        private Dictionary<Rect, NodeConnection> connectionLocations = new Dictionary<Rect, NodeConnection>();

        public Diagram()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OnDiagramLoaded);
        }

        private void OnRefreshHighlight()
        {
            InvalidateVisual();
        }

        private void OnDiagramLoaded(object sender, RoutedEventArgs e)
        {
            DData.Width = ActualWidth;
            DData.Height = ActualHeight;

            DData.ForcePropertyChanged += PropertyChangedCallback;
            VM.RefreshDiagramHighlightEvent += OnRefreshHighlight;
        }

        protected override async void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            connectionLocations.Clear();

            dc.DrawRectangle(bgBrush, bgPen, new Rect(0, 0, ActualWidth, ActualHeight));

            if (heldNode != null)
            {
                heldNode.Position = Mouse.GetPosition(this);
                heldNode.Position = ConstrainToScreen(heldNode.Position);
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
            Brush b = n.IsCurrentNode ? highlightBrush: bgBrush;
            dc.DrawEllipse(b, outlinePen, n.Position, DData.NodeSize / 2, DData.NodeSize / 2);
            if(n.IsAccepting) dc.DrawEllipse(Brushes.Transparent, outlinePen, n.Position, DData.NodeSize / 2.5, DData.NodeSize / 2.5);
            FormattedText ft = new FormattedText(n.Identifier,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Courier New"),
                DData.NodeSize / 3, outlineBrush, 1);
            dc.DrawText(ft, OffsetPoint(n.Position, -ft.Width/2, -ft.Height/2));

            if (n.IsStart)
            {
                ft = new FormattedText("START",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Courier New"),
                    DData.NodeSize / 3, outlineBrush, 1);
                dc.DrawText(ft, OffsetPoint(n.Position, -ft.Width/2, -DData.NodeSize*0.8 - ft.Height/2));
                DrawArrwohead(
                    dc,
                    OffsetPoint(n.Position, 0, -DData.NodeSize / 2),
                    OffsetPoint(n.Position, 0, -DData.NodeSize) - n.Position);
            }
        }

        private void DrawNodeConnection(DrawingContext dc, NodeConnection con)
        {
            if (con.IsSelfReferencing())
            {
                Node n = con.Node1;

                StreamGeometry sg = new StreamGeometry();
                using (StreamGeometryContext ctx = sg.Open())
                {
                    ctx.BeginFigure(
                        OffsetPoint(n.Position, DData.NodeSize / 2, 0),
                        true, false);
                    ctx.ArcTo(
                        OffsetPoint(n.Position, 0, DData.NodeSize / 2),
                        new Size(DData.NodeSize, DData.NodeSize/3),
                        0.0, true, SweepDirection.Clockwise, true, true);
                }
                sg.Freeze();
                dc.DrawGeometry(Brushes.Transparent, outlinePen, sg);
                DrawConnectionTextsAround(dc, con, OffsetPoint(n.Position, DData.NodeSize * 1.2, DData.NodeSize / 4));
                DrawArrwohead(
                    dc,
                    OffsetPoint(n.Position, DData.NodeSize / 2, 0),
                    new Vector(1, 0));
            }
            else
            {
                if (con.OpposedConnection == null)
                {
                    dc.DrawLine(outlinePen,
                        PointOnBorderTowards(con.Node1, con.Node2),
                        PointOnBorderTowards(con.Node2, con.Node1));
                    DrawArrwohead(
                        dc,
                        PointOnBorderTowards(con.Node2, con.Node1),
                        con.Node1.Position - con.Node2.Position);
                    DrawConnectionTextsAround(dc, con, CenterPoint(con));
                }
                else
                {
                    Vector nodeVector = con.Node2.Position - con.Node1.Position;
                    Point center = con.Node1.Position + nodeVector / 2;
                    Point pRight = center + MNormalize(PerpVector(nodeVector)) * DData.NodeSize * 0.7;
                    Point pLeft = center + MNormalize(PerpVector(-nodeVector)) * DData.NodeSize * 0.7;

                    //render con
                    dc.DrawLine(outlinePen,
                        PointOnBorderTowards(con.Node1, pRight), pRight);
                    dc.DrawLine(outlinePen,
                        PointOnBorderTowards(con.Node2, pRight), pRight);
                    DrawArrwohead(dc,
                        PointOnBorderTowards(con.Node2, pRight),
                        pRight - con.Node2.Position);
                    DrawConnectionTextsAround(dc, con, pRight);

                    //render opposed con
                    NodeConnection oCon = con.OpposedConnection;
                    dc.DrawLine(outlinePen,
                        PointOnBorderTowards(oCon.Node1, pLeft), pLeft);
                    dc.DrawLine(outlinePen,
                        PointOnBorderTowards(oCon.Node2, pLeft), pLeft);
                    DrawArrwohead(dc,
                        PointOnBorderTowards(oCon.Node2, pLeft),
                        pLeft - oCon.Node2.Position);
                    DrawConnectionTextsAround(dc, oCon, pLeft);
                }
            }
        }
        private void DrawConnectionTextsAround(DrawingContext dc, NodeConnection con, Point CenterOfMass)
        {
            int collinearCount = con.CollinearConnections.Count;
            double rowHeight = new FormattedText("|", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Courier New"), DData.NodeSize / 3, outlineBrush, 1).Height;
            double spacing = 5;
            double totalHeight = rowHeight * collinearCount + (collinearCount - 1) * spacing;
            for (int i = 0; i < collinearCount; i++)
            {
                DrawConnectionText(dc, con.CollinearConnections[i],
                    OffsetPoint(CenterOfMass, 0,
                    -totalHeight / 2 + i * (rowHeight + spacing) + rowHeight / 2));
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
            Brush b = con.IsCurrentTransition ? highlightBrush : accentBrush;
            dc.DrawRectangle(b, null, new Rect(new Size(fText.Width, fText.Height)));
            dc.DrawText(fText, new Point(0,0));
            dc.Pop();

            Rect r = new Rect(
                new Point(textCenter.X - fText.Width / 2, textCenter.Y - fText.Height / 2),
                new Point(textCenter.X + fText.Width / 2, textCenter.Y + fText.Height / 2));
            try { connectionLocations.Add(r, con); }
            catch (Exception) { }
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

        private Point PointOnBorderTowards(Node n, Point p2)
        {
            Vector direction = p2 - n.Position;
            direction.Normalize();
            return n.Position + direction * DData.NodeSize / 2;
        }

        private void DrawArrwohead(DrawingContext dc, Point tip, Vector direction)
        {
            direction.Normalize();
            Point corner1 = tip + direction * DData.NodeSize / 6 +
                PerpVector(direction) * DData.NodeSize / 12;
            Point corner2 = tip + direction * DData.NodeSize / 6 -
                PerpVector(direction) * DData.NodeSize / 12;

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

        private Vector PerpVector(Vector v)
        {
            return new Vector(-v.Y, v.X);
        }

        private async void ArangeDiagram(int maxIterations = 5000, double stopForce = 0.2)
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
                    n.Position = ConstrainToScreen(Vector.Add(resultingVectors[ctr], n.Position));
                    ctr++;
                }

                if (VM.AnimateDiagram)
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
                if (!(dir.X == 0 && dir.Y == 0)) dir.Normalize();
                return dir * gravityStrength;
            }
        }
        private Point ConstrainToScreen(Point p)
        {
            var rand = new Random();
            double x = Math.Clamp(p.X, DData.NodeSize / 2, ActualWidth - DData.NodeSize / 2);
            double y = Math.Clamp(p.Y, DData.NodeSize / 2, ActualHeight - DData.NodeSize / 2);
            if (p.X == double.NaN) p.X = rand.Next((int)DData.Width);
            if (p.Y == double.NaN) p.Y = rand.Next((int)DData.Height);
            return new Point(x, y);
        }

        private Point OffsetPoint(Point p, double offX, double offY)
        {
            return new Point(p.X + offX, p.Y + offY);
        }

        private Vector SubtractPositions(Node n1, Node n2)
        {
            return Vector.Subtract((Vector)n1.Position, (Vector)n2.Position);
        }

        private Vector MNormalize(Vector v)
        {
            v.Normalize();
            return v;
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

            found = false;
            foreach (Rect r in connectionLocations.Keys)
            {
                if (r.Contains((Point)mouseClickpos)){
                    rightClickedConnection = connectionLocations[r];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                rightClickedConnection = null;
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
                VM.AddTransition(rightClickedNode.State, nodeReleasedOver.State);
            }
            HandleContextMenuButtons();
        }

        private void HandleContextMenuButtons()
        {
            if (rightClickedNode != null) {
                edit_state_btn.IsEnabled = true;
                remove_state_btn.IsEnabled = true;
                add_transition_btn.IsEnabled = true;
            } else {
                edit_state_btn.IsEnabled = false;
                remove_state_btn.IsEnabled = false;
                add_transition_btn.IsEnabled = false;
            }

            if (rightClickedConnection != null) {
                edit_transition_btn.IsEnabled = true;
                remove_transition_btn.IsEnabled = true;
            } else { 
                edit_transition_btn.IsEnabled = false;
                remove_transition_btn.IsEnabled = false;
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            heldNode = null;
        }

        private void add_state_btn_Click(object sender, RoutedEventArgs e)
        {
            DData.AddNodePoint = (Point)mouseClickpos;
            VM.AddState();
        }

        private void edit_state_btn_Click(object sender, RoutedEventArgs e)
        {
            if (rightClickedNode != null)
            {
                VM.EditState(rightClickedNode.State);
            }
        }

        private void remove_state_btn_Click(object sender, RoutedEventArgs e)
        {
            if(rightClickedNode != null)
            {
                VM.RemoveState(rightClickedNode.State);
            }
        }

        private void add_transition_btn_Click(object sender, RoutedEventArgs e)
        {
            if (rightClickedNode != null)
            {
                VM.AddTransition(rightClickedNode.State, rightClickedNode.State);
            }
        }

        private void edit_transition_btn_Click(object sender, RoutedEventArgs e)
        {
            if (rightClickedConnection != null)
            {
                VM.EditTransition(rightClickedConnection.Transition);
            }
        }

        private void remove_transition_btn_Click(object sender, RoutedEventArgs e)
        {
            if (rightClickedConnection != null)
            {
                VM.RemoveTransition(rightClickedConnection.Transition);
            }
        }

        private void arrange_btn_Click(object sender, RoutedEventArgs e)
        {
            ArangeDiagram();
        }
    }
}
