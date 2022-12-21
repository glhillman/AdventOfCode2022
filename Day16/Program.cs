// See https://aka.ms/new-console-template for more information
using Day16;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using System.Text;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    Dictionary<string, Valve> _valves = new();

    public DayClass()
    {
        LoadData();

        // now that all valves are defined, populate the ConnectedValves in each valve object
        foreach (Valve valve in _valves.Values)
        {
            foreach (string valveid in valve.ConnectedValveIds)
            {
                if (_valves.ContainsKey(valveid))
                {
                    valve.ConnectedValves[valveid] = _valves[valveid];
                    valve.Distances[valveid] = 1;
                }
            }
        }

        // valves have all connections that are only 1 away
        // now iteratively flesh out the Distances from every valve to every other valve
        while (_valves.Sum(v => v.Value.Distances.Count) < _valves.Count * (_valves.Count-1))
        {
            foreach (Valve src in _valves.Values)
            {
                foreach (Valve dst in _valves.Values)
                {
                    if (src != dst)
                    {
                        if (src.Distances.ContainsKey(dst.Id) == false)
                        {
                            // our src doesn't contain distance info for dst
                            // look through our connections for a one-away
                            foreach (string subValveId in src.Distances.Keys)
                            {
                                if (_valves[subValveId].ConnectedValves.ContainsKey(dst.Id))
                                {
                                    // found a linkage!
                                    src.Distances[dst.Id] = src.Distances[subValveId] + _valves[subValveId].Distances[dst.Id];
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Part1()
    {
        int totalFlow = 0;
        var valvesWithFlow = _valves.Where(v => v.Value.FlowRate > 0);

        totalFlow = GrindOutBestFlow(30, _valves["AA"], valvesWithFlow);

        Console.WriteLine("Part1: {0}", totalFlow);
    }

    public void Part2()
    {
        List<KeyValuePair<string, Valve>> valvesWithFlow = _valves.Where(v => v.Value.FlowRate > 0).ToList();
        int maxLeft = int.MinValue;
        int maxRight = int.MinValue;
        int maxTotal = int.MinValue;
        int count = valvesWithFlow.Count();
        int nPerSide = count / 2;

        // get the permutations to try
        (List<List<KeyValuePair<string, Valve>>> leftPerms, List<List<KeyValuePair<string, Valve>>> rightPerms) = JeffPerm(valvesWithFlow, nPerSide);
        
        List <KeyValuePair<string, Valve>> leftSet = new();
        List<KeyValuePair<string, Valve>> rightSet = new();

        for (int i = 0; i < leftPerms.Count; i++)
        {
            leftSet = leftPerms[i];
            rightSet = rightPerms[i];

            maxLeft = GrindOutBestFlow(26, _valves["AA"], leftSet);
            maxRight = GrindOutBestFlow(26, _valves["AA"], rightSet);
            maxTotal = int.Max(maxTotal, (maxLeft + maxRight));
        }
        Console.WriteLine("Part2: {0}", maxTotal);
    }

    // Jeff pointed me to this permutation algorithm. 
    public (List<List<KeyValuePair<string, Valve>>>, List<List<KeyValuePair<string, Valve>>>) JeffPerm(List<KeyValuePair<string, Valve>> valves, int nPerSideMin)
    {
        List<List<KeyValuePair<string, Valve>>> aPerms = new List<List<KeyValuePair<string, Valve>>>();
        List<List<KeyValuePair<string, Valve>>> bPerms = new List<List<KeyValuePair<string, Valve>>>();
        bool[] flags = new bool[valves.Count()];
        for (int i = 0; i != valves.Count();)
        {
            List<KeyValuePair<string, Valve>> a = new List<KeyValuePair<string, Valve>>();
            List<KeyValuePair<string, Valve>> b = new List<KeyValuePair<string, Valve>>();
            for (int j = 0; j < valves.Count(); j++)
            {
                if (flags[j])
                {
                    a.Add(valves[j]);
                }
                else
                {
                    b.Add(valves[j]);
                }
            }
            // inspect a & b here
            if (a.Count == nPerSideMin)
            {
                aPerms.Add(a);
                bPerms.Add(b);
            }
            // reset flags in a very weird way...
            for (i = 0; i < valves.Count() && !(flags[i] = !flags[i]); i++) ;
        }

        return (aPerms, bPerms);
    }
    
    public int GrindOutBestFlow(int timeRemaining, Valve currentValve, IEnumerable<KeyValuePair<string, Valve>> valvesWithFlow)
    {
        // valvesWithFlow will be reduced on each recursive call - all combinations
        int totalFlow = 0;
        if (timeRemaining > 0)
        {
            foreach (KeyValuePair<string, Valve> v in valvesWithFlow)
            {
                var reducedValvesWithFlow = valvesWithFlow.Where(v2 => v2.Key != v.Key);
                int reducedTimeRemaining = timeRemaining - currentValve.Distances[v.Key] - 1; // distance + 1 to open valve
                int flow = reducedTimeRemaining * v.Value.FlowRate + GrindOutBestFlow(reducedTimeRemaining, v.Value, reducedValvesWithFlow);
                totalFlow = int.Max(totalFlow, flow);
            }
        }
        return totalFlow;
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] tokens = line.Replace("Valve ", "").Replace(" has flow rate=", ",").Replace("; tunnels lead to valves ", ",").Replace("; tunnel leads to valve ", ",").Replace(" ", "").Split(',');
                _valves[tokens[0]] = new Valve(tokens[0], int.Parse(tokens[1]), tokens.TakeLast(tokens.Length-2));
            }

            file.Close();
        }
    }
}
