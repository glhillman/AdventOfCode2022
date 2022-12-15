// See https://aka.ms/new-console-template for more information
using System;
using System.Drawing;
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
    enum CellType
    {
        Wall,
        Air,
        Sand
    };

    Dictionary<(int x, int y), CellType> _cave;
    int _minX = 0;
    int _maxX = 0;
    int _maxY = 0;

    public DayClass()
    {
        LoadData();
        _minX = _cave.Min(p => p.Key.x);
        _maxX = _cave.Max(p => p.Key.x);
        _maxY = _cave.Max(p => p.Key.y);
    }

    public void Part1()
    {
        int sandResting = 0;

        while (DropSand(false))
        {
            sandResting++;
        }

        Console.WriteLine("Part1: {0}", sandResting);
    }

    public void Part2()
    {
        LoadData();

        int sandResting = 0;

        while (DropSand(true))
        {
            sandResting++;
        }
        sandResting++; // count the plug at the top

        Console.WriteLine("Part2: {0}", sandResting);
    }

    private bool DropSand(bool hasFloor)
    {
        int x;
        int y;

        (x, y) = BounceDown((500, 0), hasFloor);

        if (x > 0 && y > 0)
        {
            _cave[(x, y)] = CellType.Sand;
        }
        return (x > 0 && y > 0);
    }

    private (int x, int y) BounceDown((int x, int y) point, bool hasFloor)
    {
        bool moved = false;
        do
        {
            if (hasFloor)
            {
                if (_cave.ContainsKey((500, 0)))
                {
                    return ((-1, -1)); // not nuts about returning from the middle, but oh well
                }
            }
            else
            {
                if (point.x < _minX || point.x > _maxX || point.y > _maxY)
                {
                    return ((-1, -1)); // not nuts about returning from the middle, but oh well
                }
            }
            moved = false;
            // if we are approaching the "floor", add floor tiles to the right & left
            if (point.y + 1 == _maxY + 2)
            {
                _cave[(point.x, point.y + 1)] = CellType.Wall;
                _cave[(point.x-1, point.y + 1)] = CellType.Wall;
                _cave[(point.x+1, point.y + 1)] = CellType.Wall;
            }
            // try straight down
            if (_cave.ContainsKey((point.x, point.y + 1)) == false)
            {
                point.y++;
                moved = true;
            }
            // try left
            if (moved == false)
            {
                if (_cave.ContainsKey((point.x - 1, point.y + 1)) == false)
                {
                    point.x--;
                    point.y++;
                    moved = true;
                }
            }
            // try right
            if (moved == false)
            {
                if (_cave.ContainsKey((point.x + 1, point.y + 1)) == false)
                {
                    point.x++;
                    point.y++;
                    moved = true;
                }
            }
        } while (moved);

        return (point.x, point.y);
    }

    private void DumpCave()
    {
        for (int y = 0; y <= _maxY; y++)
        { 
            for (int x = _minX; x <= _maxX; x++)
            {
                if (x == 500 && y == 0)
                {
                    Console.Write('+');
                }
                else
                {
                    Console.Write(_cave.ContainsKey((x, y)) ? _cave[(x, y)] == CellType.Wall ? '#' : 'o' : '.');
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            _cave = new();
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] allPoints = line.Replace(" ", "").Split("->");
                for (int i = 0; i < allPoints.Length-1; i++)
                {
                    int x1;
                    int y1;
                    int x2;
                    int y2;

                    string[] xy = allPoints[i].Split(',');
                    x1 = int.Parse(xy[0]);
                    y1 = int.Parse(xy[1]);
                    xy = allPoints[i+1].Split(",");
                    x2 = int.Parse(xy[0]);
                    y2 = int.Parse(xy[1]);
                    int xStep = 0;
                    int yStep = 0;
                    if (x1 == x2)
                    {
                        // vertical line
                        yStep = y1 - y2 < 0 ? 1 : -1;
                    }
                    else if (y1 == y2)
                    {
                        // horizontal line
                        xStep = x1 - x2 < 0 ? 1 : -1;
                    }
                    else
                    {
                        throw new Exception("Unexpected line segment!");
                    }
                    while ((x1,y1) != (x2,y2))
                    {
                        _cave[(x1,y1)] = CellType.Wall;
                        x1 += xStep;
                        y1 += yStep;
                    }
                    _cave[(x2,y2)] = CellType.Wall;
                }
            }

            file.Close();
        }
    }

}
