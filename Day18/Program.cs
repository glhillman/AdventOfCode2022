// See https://aka.ms/new-console-template for more information
using Day18;
using System.Runtime.InteropServices;

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
    Dictionary<(int x, int y, int z), Cube> _cubes = new();
    int _minX;
    int _minY;
    int _minZ;
    int _maxX;
    int _maxY;
    int _maxZ;
    (int x, int y, int z) _minBound;
    (int x, int y, int z) _maxBound;
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        foreach (var key in _cubes.Keys)
        {
            foreach (var neighbor in Neighbors(key))
            {
                if (_cubes.ContainsKey(neighbor))
                {
                    _cubes[neighbor].TouchCount++;
                }
            }
        }

        int sum = _cubes.Sum(c => c.Value.UntouchedCount);
        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        _minBound = (_minX - 1, _minY - 1, _minZ - 1);
        _maxBound = (_maxX + 1, _maxY + 1, _maxZ + 1);

        List<(int x, int y, int z)> floodedCubes = FloodCubes(_minBound);
        List<(int x, int y, int z)> allNeighbors = new();
        
        foreach (var key in _cubes.Keys)
        {
            foreach (var neighbor in Neighbors(key))
            {
                allNeighbors.Add(neighbor);
            }
        }

        int exteriorSurfaceArea = 0;
        foreach (var neighbor in allNeighbors)
        {
            exteriorSurfaceArea += floodedCubes.Contains(neighbor) ? 1 : 0;
        }

        Console.WriteLine("Part2: {0}", exteriorSurfaceArea);
    }

    private List<(int x, int y, int z)> FloodCubes((int x, int y, int z) from)
    {
        List<(int x, int y, int z)> flooded = new();
        Queue<(int x, int y, int z)> queue = new();

        flooded.Add(from);
        queue.Enqueue(from);

        while (queue.Any())
        {
            var wet = queue.Dequeue();
            foreach (var neighbor in Neighbors(wet))
            {
                if (flooded.Contains(neighbor) == false && InBounds(neighbor) && _cubes.ContainsKey(neighbor) == false)
                {
                    flooded.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return flooded;
    }

    private bool InBounds ((int x, int y, int z) pos)
    {
        return pos.x >= _minBound.x && pos.x <= _maxBound.x && pos.y >= _minBound.y && pos.y <= _maxBound.y && pos.z >= _minBound.z && pos.z <= _maxBound.z;
    }
    IEnumerable<(int x, int y, int z)> Neighbors((int x, int y, int z) pos)
    {
        return new[]
        {
            pos with { x = pos.x - 1 },
            pos with { x = pos.x + 1 },
            pos with { y = pos.y - 1 },
            pos with { y = pos.y + 1 },
            pos with { z = pos.z - 1 },
            pos with { z = pos.z + 1 }
        };
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string[] lines = File.ReadAllLines(inputFile);
            foreach (string line in lines)
            {
                string[] split = line.Split(',');
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                int z = int.Parse(split[2]);
                Cube cube = new Cube(x, y, z);
                _cubes[cube.Coords] = cube;
            }

            _minX = _cubes.Min(c => c.Value.X);
            _minY = _cubes.Min(c => c.Value.Y);
            _minZ = _cubes.Min(c => c.Value.Z);
            _maxX = _cubes.Max(c => c.Value.X);
            _maxY = _cubes.Max(c => c.Value.Y);
            _maxZ = _cubes.Max(c => c.Value.Z);
        }
    }

}
