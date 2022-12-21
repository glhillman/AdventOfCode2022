using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Valve
    {
        public Valve(string id, int flowRate, IEnumerable<string> connectedValves)
        {
            Id = id;
            FlowRate = flowRate;
            ConnectedValveIds = new();
            foreach (string valve in connectedValves)
            {
                ConnectedValveIds.Add(valve);
            }
            ConnectedValves = new(); // these will be resolved after all valves have been defined
            Distances = new(); // shortest distance to any other valve
            On = false;
        }

        public string Id { get; private set; }
        public int FlowRate { get; private set;}
        public List<string> ConnectedValveIds { get; private set;}
        public Dictionary<string, Valve> ConnectedValves { get; private set;}
        public Dictionary<string, int> Distances { get; private set;}
        public bool On { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(string valve in ConnectedValveIds)
            {
                sb.Append(valve + " ");
            }
            return string.Format("Id: {0}, Flowrate: {1}, On: {2}, ConnectedValves: {2}", Id, FlowRate, On, sb.ToString());
        }
    }
}
