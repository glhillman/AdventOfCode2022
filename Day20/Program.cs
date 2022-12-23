// See https://aka.ms/new-console-template for more information
using Day20;
using System;
using System.Diagnostics.CodeAnalysis;

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
    LinkedNodes _nodes;

    public DayClass()
    {
    }

    public void Part1()
    {
        LoadData(1);

        for (int i = 0; i < _nodes.NodeCount; i++)
        {
            _nodes.MixNode(i);
        }

        long groveCoords = _nodes.GetGroveCoordinates();

        Console.WriteLine("Part1: {0}", groveCoords);
    }

    public void Part2()
    {
        LoadData(811589153);

        for (int mix = 0; mix < 10; mix++)
        {
            for (int i = 0; i < _nodes.NodeCount; i++)
            {
                _nodes.MixNode(i);
            }

        }
        long groveCoords = _nodes.GetGroveCoordinates();

        Console.WriteLine("Part2: {0}", groveCoords);
    }


    private void LoadData(long decryptKey)
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string[] strings= File.ReadAllLines(inputFile);
            _nodes = new LinkedNodes();
            for (int i = 0; i < strings.Length; i++)
            {
                _nodes.Add(long.Parse(strings[i]) * decryptKey);
            }
        }
    }
}
