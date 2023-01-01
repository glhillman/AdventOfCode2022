// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Runtime.CompilerServices;

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
    private HashSet<(int row, int col)> _elves;

    private enum Compass
    {
        N,
        S,
        E,
        W,
        NE,
        NW,
        SE,
        SW
    };
    private Dictionary<Compass, (int row, int col)> _Offsets = new();

    public DayClass()
    {
        _Offsets[Compass.N] = (-1, 0);
        _Offsets[Compass.S] = (1, 0);
        _Offsets[Compass.E] = (0, 1);
        _Offsets[Compass.W] = (0, -1);
        _Offsets[Compass.NE] = (-1, 1);
        _Offsets[Compass.NW] = (-1, -1);
        _Offsets[Compass.SE] = (1, 1);
        _Offsets[Compass.SW] = (1, -1);
    }

    public void Part1()
    {
        LoadData();
        //DumpElves("test");

        int anchorIndex = 0;

        for (int i = 0; i < 10; i++)
        {
            MoveElves(anchorIndex++);
        }

        (int rowMin, int rowMax, int colMin, int colMax) rect = BoundingRectangle();

        int total = (rect.rowMax - rect.rowMin + 1) * (rect.colMax - rect.colMin + 1);
        int empties = total - _elves.Count;
        Console.WriteLine("Part1: {0}", empties);
    }

    public void Part2()
    {
        LoadData();
        int anchorIndex = 0;
        int nMoves = 0;

        while (MoveElves(anchorIndex++))
        {
            nMoves++;
        }
        ++nMoves;

        Console.WriteLine("Part2: {0}", nMoves);
    }

    private bool MoveElves(int anchorIndex)
    {
        bool elvesWereMoved = false;

        Dictionary<(int srcRow, int srcCol), List<(int dstRow, int dstCol)>> proposals = new(); // key is target location, value is list of source locations 

        foreach ((int row, int col) elf in _elves)
        {
            int index = anchorIndex;
            if (HasNeighbors(elf))
            {
                // loop through all 4 rules looking for empties in that quadrant
                for (int tries = 0; tries < 4; tries++)
                {
                    List<Compass> offsets = OffsetsToCheck(index++);
                    if (QuadrantIsEmpty(elf, offsets))
                    {
                        (int row, int col) prop = ApplyOffset(elf, offsets[0]); // if successful, the target location is in offset[0] (N, S, E, W)

                        if (proposals.ContainsKey(prop) == false)
                        {
                            proposals[prop] = new();
                        }
                        proposals[prop].Add(elf);
                        break;
                    }
                }
            }
        }
        foreach (var canMove in proposals.Where(p => p.Value.Count == 1)) // count > 1 represents collisions in the proposals - ignore them
        {
            _elves.Add(canMove.Key);
            _elves.Remove(canMove.Value[0]);
            elvesWereMoved = true;
        }

        return elvesWereMoved;
    }
    private bool QuadrantIsEmpty((int elfRow, int elfCol) elf, List<Compass> offsets)
    {
        bool targetIsEmpty = true;

        // check the three offets relative to the current elf to see if we can move
        foreach (Compass compass in offsets)
        {
            (int row, int col) target = ApplyOffset(elf, compass);
            if (_elves.Contains(target))
            {
                targetIsEmpty = false;
                break;
            }
        }

        return targetIsEmpty;
    }

    private (int row, int col) ApplyOffset((int row, int col) elf, Compass compass)
    {
        return (elf.row + _Offsets[compass].row, elf.col + _Offsets[compass].col);
    }

    private bool HasNeighbors((int row, int col) pos)
    {
        bool hasNeighbors = false;
        
        // verify that there is at least one neighbor crowding us. 
        for (int row = pos.row -1; !hasNeighbors && row <= pos.row + 1; row++)
        {
            for (int col = pos.col - 1; !hasNeighbors && col <= pos.col+1; col++)
            {
                if ((row, col) != pos)
                {
                    hasNeighbors = _elves.Contains((row, col));
                }
            }
        }

        return hasNeighbors;
    }
    
    private List<Compass>  OffsetsToCheck(int index)
    {
        List<Compass> offsets = new();
        switch (index % 4)
        {
            case 0:
                offsets.Add(Compass.N);
                offsets.Add(Compass.NE);
                offsets.Add(Compass.NW);
                break;
            case 1:
                offsets.Add(Compass.S);
                offsets.Add(Compass.SE);
                offsets.Add(Compass.SW);
                break;
            case 2:
                offsets.Add(Compass.W);
                offsets.Add(Compass.NW);
                offsets.Add(Compass.SW);
                break;
            case 3:
                offsets.Add(Compass.E);
                offsets.Add(Compass.NE);
                offsets.Add(Compass.SE);
                break;
        }

        return offsets;
    }

    private void DumpElves(string msg)
    {
        (int rowMin, int rowMax, int colMin, int colMax) rect = BoundingRectangle();

        Console.WriteLine(msg);
        for (int row = rect.rowMin; row <= rect.rowMax; row++)
        {
            for (int col = rect.colMin; col <= rect.colMax; col++)
            {
                Console.Write(_elves.Contains((row, col)) ? '#' : '.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private (int rowMin, int rowMax, int colMin, int colMax) BoundingRectangle()
    {
        return (_elves.Min(e => e.row), _elves.Max(e => e.row), _elves.Min(e => e.col), _elves.Max(e => e.col));
    }
    private void LoadData()
    {
        _elves = new();
        string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt");
        for (int row = 0; row < lines.Length; row++)
        {
            string line = lines[row];
            for (int col = 0; col < line.Length; col++)
            {
                if (line[col] == '#')
                {
                    _elves.Add((row, col));
                }
            }
        }
    }

}
