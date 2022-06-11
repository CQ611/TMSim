using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMSim.Core;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        public static DiagramData UpdateDiagramData(DiagramData DData, TuringMachine TM)
        {
            var rand = new Random();
            DiagramData tmpDData = new DiagramData();

            int ctr = 0;
            foreach (TuringState ts in TM.States)
            {
                Point pos;
                if (DData.Nodes.Keys.Contains(ts.Identifier))
                {
                    pos = DData.Nodes[ts.Identifier].Position;
                }
                else
                {
                    double theta = (ctr + rand.NextDouble() * 0.1) / TM.States.Count * 2 * Math.PI;
                    double r = 3 * DData.NodeSize;
                    pos = new Point(
                        DData.Width/2 + r * Math.Cos(theta),
                        DData.Height/2 + r * Math.Sin(theta));
                }
                tmpDData.Nodes.Add(ts.Identifier, new Node(ts, pos, ts == TM.CurrentState));
                ctr++;
            }


            DData.Nodes = tmpDData.Nodes;
            DData.Connections.Clear();
            foreach (TuringTransition tt in TM.Transitions)
            {
                NodeConnection nc = new NodeConnection(
                    tt,
                    DData.Nodes[tt.Source.Identifier],
                    DData.Nodes[tt.Target.Identifier]
                    );
                bool foundParentCon = false;                
                foreach(NodeConnection nc2 in DData.Connections)
                {
                    if (nc2.IsCollinear(nc))
                    {
                        nc2.CollinearConnections.Add(nc);
                        foundParentCon = true;
                        break;
                    }
                    if(nc2.OpposedConnection != null)
                    {
                        if (nc2.OpposedConnection.IsCollinear(nc))
                        {
                            nc2.OpposedConnection.CollinearConnections.Add(nc);
                            foundParentCon = true;
                            break;
                        }
                    }
                }
                if (!foundParentCon) {
                    foreach (NodeConnection nc2 in DData.Connections)
                    {
                        if (nc2.IsOpposed(nc))
                        {
                            nc2.OpposedConnection = nc;
                            foundParentCon = true;
                            break;
                        }
                    }
                }
                if (!foundParentCon) DData.Connections.Add(nc);
            }
            return DData;
        }


        public delegate void RefreshDiagramHighlight();
        public event RefreshDiagramHighlight RefreshDiagramHighlightEvent;

        private void RefreshDiagramData()
        {
            Node curNode = DData.Nodes.Values.FirstOrDefault((n) => n.IsCurrentNode);
            if (curNode != null) curNode.IsCurrentNode = false;
            NodeConnection curCon = FindConnectionWhere((c) => c.IsCurrentTransition);
            if (curCon != null) curCon.IsCurrentTransition = false;

            if (HighlightCurrentState)
            {
                Node nextNode = DData.Nodes.Values.FirstOrDefault((n) => n.State == TM.CurrentState);
                if (nextNode != null) nextNode.IsCurrentNode = true;
                NodeConnection nextCon = FindConnectionWhere((c) => c.Transition == TM.CurrentTransition);
                if (nextCon != null) nextCon.IsCurrentTransition = true;
            }
            RefreshDiagramHighlightEvent?.Invoke();
        }

        private NodeConnection FindConnectionWhere(Predicate<NodeConnection> match)
        {
            //Function for iterating through the tree structures of NodeConnections
            foreach (NodeConnection nc in DData.Connections)
            {
                if (match(nc)) return nc;
                foreach (NodeConnection ncCol in nc.CollinearConnections)
                {
                    if (match(ncCol)) return ncCol;
                }
                if (nc.OpposedConnection != null)
                {
                    if (match(nc.OpposedConnection)) return nc.OpposedConnection;
                    foreach (NodeConnection ncOps in nc.OpposedConnection.CollinearConnections)
                    {
                        if (match(ncOps)) return ncOps;
                    }
                }
            }
            return null;
        }
    }
}
