using System;
using System.Collections.Generic;
using System.Linq;
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

            HashSet<string> newStates = new HashSet<string>(); //set of identifiers of states
            HashSet<string> oldStates = new HashSet<string>(); //set of identifiers of states

            TM.States.ForEach(ts => newStates.Add(ts.Identifier));
            DData.Nodes.Values.ToList().ForEach(n => oldStates.Add(n.Identifier));

            HashSet<string> addedStates = newStates.Except(oldStates).ToHashSet();
            HashSet<string> deletedStates = oldStates.Except(newStates).ToHashSet();

            deletedStates.ToList().ForEach(id => DData.Nodes.Remove(id));
            foreach(string identfier in addedStates)
            {
                TuringState ts = TM.States.First(x => x.Identifier == identfier);
                Node n = new Node(ts, DData.AddNodePoint,
                    TM.StartState == ts, TM.EndStates.Contains(ts));
                DData.Nodes.Add(n.Identifier, n);
            }

            DData.Connections.Clear();
            foreach (TuringTransition tt in TM.Transitions)
            {
                NodeConnection nc = new NodeConnection(
                    tt,
                    DData.Nodes[tt.Source.Identifier],
                    DData.Nodes[tt.Target.Identifier]
                    );
                DData.Connections.Add(nc);
            }

            return DData;
        }
    }
}
