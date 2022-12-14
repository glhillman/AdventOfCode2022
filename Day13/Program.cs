// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Xml.Linq;

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
    List<JsonNode> _packets = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int packetIndex = 0;
        int pairNum = 1;
        int pairSum = 0;
        
        while (packetIndex < _packets.Count)
        {
            pairSum += ComparePackets(_packets[packetIndex++], _packets[packetIndex++]) < 0 ? pairNum : 0;
            pairNum++;
        }

        Console.WriteLine("Part1: {0}", pairSum);
    }

    public void Part2()
    {
        _packets.Add(JsonNode.Parse("[[2]]"));
        _packets.Add(JsonNode.Parse("[[6]]"));
        
        _packets.Sort((left, right) => ComparePackets(left, right));

        int decoderKey = (_packets.FindIndex(p => p.ToJsonString() == "[[2]]") + 1) * (_packets.FindIndex(p => p.ToJsonString() == "[[6]]") + 1);

        Console.WriteLine("Part2: {0}", decoderKey);
    }

    private int ComparePackets(JsonNode left, JsonNode right)
    {
        int diff = 0;

        if (left is JsonValue && right is JsonValue)
        {
            return (int)left - (int)right;
        }
        else
        {
            JsonArray jsonArrayLeft;
            JsonArray jsonArrayRight; 
            if (left is JsonArray)
            {
                jsonArrayLeft = left as JsonArray;
            }
            else
            {
                jsonArrayLeft = new JsonArray((int)left);
            }
            if (right is JsonArray)
            {
                jsonArrayRight = right as JsonArray;
            }
            else
            {
                jsonArrayRight = new JsonArray((int)right);
            }

            var zipVar = Enumerable.Zip(jsonArrayLeft, jsonArrayRight); //magic matchup of elements from both arrays

            foreach (var pair in zipVar)
            {
                diff = ComparePackets(pair.First, pair.Second);
                if (diff != 0)
                {
                    return diff;
                }
            }
            if (diff == 0)
            {
                diff = jsonArrayLeft.Count - jsonArrayRight.Count;
                if (diff != 0)
                {
                    return diff;
                }
            }
        }
        return diff;
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            StreamReader file = new StreamReader(inputFile);

            string[] input = file.ReadToEnd().Split("\r\n");

            file.Close();
            foreach (string packet in input)
            {
                if (packet.Length > 0)
                {
                    _packets.Add(JsonNode.Parse(packet));
                }
            }
        }
    }
}
