// See https://aka.ms/new-console-template for more information
using Day22;
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
    Dictionary<(int row, int col), char> _map = new();
    Dictionary<int, (int minCol, int maxCol)> _rowBounds = new(); // left & right column bounds by row
    Dictionary<int, (int minRow, int maxRow)> _colBounds = new(); // top & bottom row bounds by col
    List<int> _instructions = new();
    int _maxRow = int.MinValue;
    int _maxCol = int.MinValue;
    List<Surface> _surfaces;
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        char curDirection = '>';
        (int row, int col) pos = (0, _rowBounds[0].minCol);
        
        for (int i = 0; i < _instructions.Count; i++)
        {
            int instruction = _instructions[i];
            if (instruction >= 0)
            {
                pos = Move1(instruction, pos, curDirection);        // attempt to move [instruction] steps in the current direction
            }
            else
            {
                curDirection = TurnDirection(curDirection, instruction);
            }
        }

        int password = (pos.row + 1) * 1000 + (pos.col + 1) * 4 + Facing(curDirection);

        Console.WriteLine("Part1: {0}", password);
    }

    public void Part2()
    {
        _surfaces = LoadSurfaces();

        char curDirection = '>';
        (int rowDelta, int colDelta) delta = GetDeltas(curDirection);
        (int row, int col) pos = (0, _rowBounds[0].minCol);

        Surface surface = _surfaces[0];

        for (int i = 0; i < _instructions.Count; i++)
        {
            int instruction = _instructions[i];
            if (instruction >= 0)
            {
                (Surface surf, char direction, int row, int col) = Move2(surface, instruction, pos, delta, curDirection);        // attempt to move [instruction] steps in the current direction
                pos = (row, col);
                curDirection = direction;
                surface = surf;
            }
            else
            {
                curDirection = TurnDirection(curDirection, instruction);
                delta = GetDeltas(curDirection);
            }
        }

        int password = (pos.row + 1) * 1000 + (pos.col + 1) * 4 + Facing(curDirection);

        Console.WriteLine("Part2: {0}", password);
    }

    private (int row, int col) Move1(int stepCount, (int curRow, int curCol) pos, char curDirection)
    {
        // movement logic for part 1 - different wrapping logic from part 2
        int newRow = pos.curRow;
        int newCol = pos.curCol;
        (int rowDelta, int colDelta) delta = GetDeltas(curDirection);
        bool blocked = false;

        for (int i = 0; !blocked && i < stepCount; i++)
        {
            newRow += delta.rowDelta;
            newCol += delta.colDelta;

            if (_map.ContainsKey((newRow, newCol)))
            {
                if (_map[(newRow, newCol)] == '#')
                {
                    newRow -= delta.rowDelta;
                    newCol -= delta.colDelta;
                    blocked = true;
                }
            }
            else
            {
                // wrap if possible
                int saveRow = newRow - delta.rowDelta;
                int saveCol = newCol - delta.colDelta;
                switch (curDirection)
                {
                    case '>':
                        newCol = _rowBounds[saveRow].minCol;
                        break;
                    case 'v':
                        newRow = _colBounds[saveCol].minRow;
                        break;
                    case '<':
                        newCol = _rowBounds[saveRow].maxCol;
                        break;
                    case '^':
                        newRow = _colBounds[saveCol].maxRow;
                        break;
                }
                if (_map[(newRow, newCol)] == '#')
                {
                    newRow = saveRow;
                    newCol = saveCol;
                    blocked = true;
                }
            }
        }

        return (newRow, newCol);
    }

    private (Surface surf, char direction, int row, int col) Move2(Surface surface, int stepCount, (int curRow, int curCol) pos, (int rowDelta, int colDelta) delta, char curDirection)
    {
        int newRow = pos.curRow;
        int newCol = pos.curCol;
        bool blocked = false;

        for (int i = 0; !blocked && i < stepCount; i++)
        {
            newRow += delta.rowDelta;
            newCol += delta.colDelta;

            // are newRow and newCol inside of the current surface?
            if (surface.Contains(newRow, newCol))
            {
                if (_map[(newRow, newCol)] == '#')
                {
                    newRow -= delta.rowDelta;
                    newCol -= delta.colDelta;
                    blocked = true;
                }
            }
            else
            {
                // nope - transition to the next surface
                newRow -= delta.rowDelta;
                newCol -= delta.colDelta;

                TransitionDefinition transitionDef = surface.Transitions[curDirection];
                Surface dstSurface = _surfaces[transitionDef.NextSurface - 1];
                (char newDirection, int tRow, int tCol) = transitionDef.Transition(newRow, newCol, dstSurface);
                if (_map[(tRow, tCol)] == '#')
                {
                    blocked = true;
                }
                else
                {
                    newRow = tRow;
                    newCol = tCol;
                    curDirection = newDirection;
                    delta = GetDeltas(curDirection);
                    surface = dstSurface;
                }
            }
        }

        return (surface, curDirection, newRow, newCol);
    }

    private char TurnDirection(char curDirection, int instruction )
    {
        if (instruction == -1)
        {
            // turn right 90 degrees
            switch (curDirection)
            {
                case '>': curDirection = 'v'; break;
                case 'v': curDirection = '<'; break;
                case '<': curDirection = '^'; break;
                case '^': curDirection = '>'; break;
            }
        }
        else
        {
            // turn left 90 degrees
            switch (curDirection)
            {
                case '>': curDirection = '^'; break;
                case '^': curDirection = '<'; break;
                case '<': curDirection = 'v'; break;
                case 'v': curDirection = '>'; break;
            }
        }
        return curDirection;
    }

    private (int rowInc, int colInc) GetDeltas(char curDirection)
    {
        switch (curDirection)
        {
            case '>': return (0, 1);
            case 'v': return (1, 0);
            case '<': return (0, -1);
            default:  return (-1, 0); // '^'
        }
    }

    private int Facing(char curDirection)
    {
        switch (curDirection)
        {
            case '>': return 0;
            case 'v': return 1;
            case '<': return 2;
            default: return 3; // '^'
        }
    }
    private List<Surface> LoadSurfaces()
    {
        List<Surface> surfaces = new();

        for (int surfNum = 1; surfNum <= 6; surfNum++)
        {
            Surface? surface = null;
            
            // these are the 4 surfaces that are adjacent to each of the six surfaces.
            // the transition definition describes how to move from one surface to the other preserving row/col offsets & movement direction
            switch (surfNum)
            {
                case 1:
                    surface = new Surface(surfNum, (0, 49), (50, 99));
                    surface.Transitions['^'] = new TransitionDefinition(1, 6, '^', '>', TransitionDefinition.RowMinToColMin);
                    surface.Transitions['v'] = new TransitionDefinition(1, 3, 'v', 'v', TransitionDefinition.RowMaxToRowMin);
                    surface.Transitions['<'] = new TransitionDefinition(1, 4, '<', '>', TransitionDefinition.ColMinToColMin);
                    surface.Transitions['>'] = new TransitionDefinition(1, 2, '>', '>', TransitionDefinition.ColMaxToColMin);
                    break;
                case 2:
                    surface = new Surface(surfNum, (0, 49), (100, 149));
                    surface.Transitions['^'] = new TransitionDefinition(2, 6, '^', '^', TransitionDefinition.RowMinToRowMax);
                    surface.Transitions['v'] = new TransitionDefinition(2, 3, 'v', '<', TransitionDefinition.RowMaxToColMax);
                    surface.Transitions['<'] = new TransitionDefinition(2, 1, '<', '<', TransitionDefinition.ColMinToColMax);
                    surface.Transitions['>'] = new TransitionDefinition(2, 5, '>', '<', TransitionDefinition.ColMaxToColMax);
                    break;
                case 3:
                    surface = new Surface(surfNum, (50, 99), (50, 99));
                    surface.Transitions['^'] = new TransitionDefinition(3, 1, '^', '^', TransitionDefinition.RowMinToRowMax);
                    surface.Transitions['v'] = new TransitionDefinition(3, 5, 'v', 'v', TransitionDefinition.RowMaxToRowMin);
                    surface.Transitions['<'] = new TransitionDefinition(3, 4, '<', 'v', TransitionDefinition.ColMinToRowMin);
                    surface.Transitions['>'] = new TransitionDefinition(3, 2, '>', '^', TransitionDefinition.ColMaxToRowMax);
                    break;
                case 4:
                    surface = new Surface(surfNum, (100, 149), (0, 49));
                    surface.Transitions['^'] = new TransitionDefinition(4, 3, '^', '>', TransitionDefinition.RowMinToColMin);
                    surface.Transitions['v'] = new TransitionDefinition(4, 6, 'v', 'v', TransitionDefinition.RowMaxToRowMin);
                    surface.Transitions['<'] = new TransitionDefinition(4, 1, '<', '>', TransitionDefinition.ColMinToColMin);
                    surface.Transitions['>'] = new TransitionDefinition(4, 5, '>', '>', TransitionDefinition.ColMaxToColMin);
                    break;
                case 5:
                    surface = new Surface(surfNum, (100, 149), (50, 99));
                    surface.Transitions['^'] = new TransitionDefinition(5, 3, '^', '^', TransitionDefinition.RowMinToRowMax);
                    surface.Transitions['v'] = new TransitionDefinition(5, 6, 'v', '<', TransitionDefinition.RowMaxToColMax);
                    surface.Transitions['<'] = new TransitionDefinition(5, 4, '<', '<', TransitionDefinition.ColMinToColMax);
                    surface.Transitions['>'] = new TransitionDefinition(5, 2, '>', '<', TransitionDefinition.ColMaxToColMax);
                    break;
                case 6:
                    surface = new Surface(surfNum, (150, 199), (0, 49));
                    surface.Transitions['^'] = new TransitionDefinition(6, 4, '^', '^', TransitionDefinition.RowMinToRowMax);
                    surface.Transitions['v'] = new TransitionDefinition(6, 2, 'v', 'v', TransitionDefinition.RowMaxToRowMin);
                    surface.Transitions['<'] = new TransitionDefinition(6, 1, '<', 'v', TransitionDefinition.ColMinToRowMin);
                    surface.Transitions['>'] = new TransitionDefinition(6, 5, '>', '^', TransitionDefinition.ColMaxToRowMax);
                    break;
            }

            if (surface != null)
            {
                surfaces.Add(surface);
            }
        }
        return surfaces;
    }

    private void LoadData()
    {
        string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt");
        string line;
        _maxRow = lines.Length - 3;
        for (int row = 0; row <= _maxRow; row++)
        {
            // create the map entries. Blank space is not represented
            int leftCol = -1;
            line = lines[row].TrimEnd();
            for (int col = 0; col < line.Length; col++)
            {
                if (line[col] != ' ')
                {
                    if (leftCol < 0)
                    {
                        leftCol = col;
                    }
                    _map[(row, col)] = line[col];
                    _maxCol = Math.Max(_maxCol, col);
                }
            }
            _rowBounds[row] = (leftCol, line.Length - 1);
        }
        // get the column boundaries
        for (int col = 0; col <= _maxCol; col++)
        {
            var column = _map.Keys.Where(p => p.col == col);
            int topRow = column.Min(p => p.row);
            int bottomRow = column.Max(p => p.row);
            _colBounds[col] = (topRow, bottomRow);
        }

        // process the steps
        line = lines[lines.Length - 1];
        StringBuilder sb = new();
        foreach (char c in line)
        {
            switch (c)
            {
                case 'R':
                    if (sb.Length > 0)
                    {
                        _instructions.Add(int.Parse(sb.ToString()));
                        sb.Clear();
                    }
                    _instructions.Add(-1);
                    break;
                case 'L':
                    if (sb.Length > 0)
                    {
                        _instructions.Add(int.Parse(sb.ToString()));
                        sb.Clear();
                    }
                    _instructions.Add(-2);
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        if (sb.Length > 0)
        {
            _instructions.Add(int.Parse(sb.ToString()));
        }
    }

}
