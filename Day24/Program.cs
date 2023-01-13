// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using static DayClass;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1And2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class DayClass
{
    const int UP = 1;
    const int DOWN = 2;
    const int LEFT = 4;
    const int RIGHT = 8;

    public record Boundary(int minRow, int maxRow, int minCol, int maxCol);
    public Boundary _boundary;
    //public List<Blizzard> _blizzardsAtStart = new();
    public Dictionary<int, Dictionary<Pos, int>> _blizzardStates = new();
    public int _lcm = 0;
    public Pos _start = new Pos(0, 0);
    public Pos _finish = new Pos(0, 0);
    public Pos _target = new Pos(0, 0);

     public DayClass()
    {
         LoadData();
        //DumpBlizzards(12);
    }

    public void Part1And2()
    {
        _start = new Pos(_boundary.minRow-1, 1);
        _finish = new Pos(_boundary.maxRow + 1, _boundary.maxCol);
        _target = _finish;

        Step step = Traverse(_start, _target, 0);
        Console.WriteLine("Part1: {0}", step.Minute);
        step = Traverse(_target, _start, step.Minute);
        step = Traverse(_start, _target, step.Minute);
        Console.WriteLine("Part2: {0}", step.Minute);
   }


    private Step Traverse(Pos start, Pos finish, int minute)
    {
        Step.target = finish; // set as static field of Step - used each time Pos is created
        Step checkStep = new Step(start, minute);
        PriorityQueue<Step, int> activeSteps = new(); 
        HashSet<Step> visited = new();

        activeSteps.Enqueue(checkStep, checkStep.Distance);

        while (activeSteps.Count > 0) //Any())
        {
            checkStep = activeSteps.Dequeue();

            //Console.WriteLine(string.Format("{0}, visited.Count: {1}, activeSteps.Count: {2}", checkStep.ToString(), visited.Count, activeSteps.Count)); ;
            if (checkStep.Pos == finish)
            {
                return checkStep; // destination!
            }

            if (visited.Add(checkStep) == false)
            {
                continue;
            }

            List<Step> openNeighbors = OpenNeighbors(checkStep, start, finish);

            foreach (Step neighborStep in openNeighbors)
            {
                activeSteps.Enqueue(neighborStep, neighborStep.Distance);
            }
        }

        return new Step(new Pos(0, 0), -1); // not found :-(
    }

    private void LoadData()
    {
        string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt");
        _boundary = new Boundary(1, lines.Length - 2, 1, lines[0].Length - 2);
        Dictionary<Pos, int> currentState = new();
        for (int row = _boundary.minRow; row <= _boundary.maxRow; row++)
        {
            for (int col = _boundary.minCol; col <= _boundary.maxCol; col++)
            {
                int bliz = 0;
                switch (lines[row][col])
                {
                    case '^': bliz = UP; break;
                    case 'v': bliz = DOWN; break;
                    case '>': bliz = RIGHT; break;
                    case '<': bliz = LEFT; break;
                }
                if (bliz != 0)
                {
                    currentState[new Pos(row, col)] = bliz;
                }
            }
        }

        // get LCM of the boundaries - state will reach initial state at that point
        _lcm = CalcLCM(_boundary.maxRow, _boundary.maxCol);
        _blizzardStates[0] = currentState;
        //DumpBlizzards(0);
        int[] directions = new int[] {UP, DOWN, LEFT, RIGHT};
        // cache all blizzard states by minute
        for (int minute = 1; minute < _lcm; minute++)
        {
            Dictionary<Pos, int> newState = new();
            foreach (KeyValuePair<Pos, int> bliz in currentState)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    int mask = bliz.Value & directions[dir];
                    if (mask != 0)
                    {
                        Pos newPos = MoveBlizzard(bliz.Key, mask, _boundary);
                        if (newState.ContainsKey(newPos))
                        {
                            newState[newPos] |= mask;
                        }
                        else
                        {
                            newState[newPos] = mask;
                        }
                    }
                }
            }
            _blizzardStates[minute] = newState;
            //DumpBlizzards(minute);
            currentState = newState;
        }
    }

    private Pos MoveBlizzard(Pos curPos, int mask, Boundary boundary)
    {
        Pos newPos;

        int temp;
        if ((mask & RIGHT) == RIGHT)
        {
            temp = curPos.col + 1;
            temp = temp > boundary.maxCol ? boundary.minCol : temp;
            newPos = new Pos(curPos.row, temp);
        }
        else if ((mask & LEFT) == LEFT)
        {
            temp = curPos.col - 1;
            temp = temp < boundary.minCol ? boundary.maxCol : temp;
            newPos = new Pos(curPos.row, temp);
        }
        else if ((mask & UP) == UP)
        {
            temp = curPos.row - 1;
            temp = temp < boundary.minRow ? boundary.maxRow : temp;
            newPos = new Pos(temp, curPos.col);
        }
        else // mask == DOWN)
        {
            temp = curPos.row + 1;
            temp = temp > boundary.maxRow ? boundary.minRow : temp;
            newPos = new Pos(temp, curPos.col);
        }

        return newPos;
    }


    public record Pos(int row, int col)
    {
        public Pos AddDelta(int rowDelta, int colDelta)
        {
            return new Pos(row + rowDelta, col + colDelta);
        }

        public override string ToString()
        {
            return string.Format("row: {0}, col: {1}", row, col);
        }
    }

    public record TimePos(int time, Pos pos)
    {
        public override string ToString()
        {
            return string.Format("time: {0}, {1}", time, pos);
        }
    }

    public class Step
    {
        public static Pos target = new Pos(0, 0);

        public Step(Pos pos, int minute)
        {
            Pos = pos;
            Minute = minute;
            Distance = Math.Abs(target.row - Pos.row) + Math.Abs(target.col - Pos.col) + minute;
        }
        public Pos Pos { get; set; }
        public int Minute { get; set; }
        public int Distance { get; set; }

        public override bool Equals(object? obj)
        {
            Step other = obj as Step;
            return other.Pos.row == Pos.row && other.Pos.col == Pos.col && other.Minute == Minute;
        }

        public override int GetHashCode()
        {
            return Pos.row.GetHashCode() ^ Pos.col.GetHashCode() ^ Minute.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}, Minute: {1}, Distance: {2}", Pos, Minute, Distance);
        }
    }

    private List<Step> OpenNeighbors(Step step, Pos start, Pos finish)
    {
        int minute = step.Minute + 1;
        List<Step> openNeighbors = new();
        Dictionary<Pos, int> blizzards = _blizzardStates[minute % _lcm];

        Pos neighborPos = step.Pos.AddDelta(-1, 0); // above
        if (OpenNeighbor(blizzards, neighborPos, start, finish))
        {
            openNeighbors.Add(new Step(neighborPos, minute));
        }

        neighborPos = step.Pos.AddDelta(1, 0); // below
        if (OpenNeighbor(blizzards, neighborPos, start, finish))
        {
            openNeighbors.Add(new Step(neighborPos, minute));
        }

        neighborPos = step.Pos.AddDelta(0, -1); // left
        if (OpenNeighbor(blizzards, neighborPos, start, finish))
        {
            openNeighbors.Add(new Step(neighborPos, minute));
        }

        neighborPos = step.Pos.AddDelta(0, 1); // right
        if (OpenNeighbor(blizzards, neighborPos, start, finish))
        {
            openNeighbors.Add(new Step(neighborPos, minute));
        }

        if (OpenNeighbor(blizzards, step.Pos, start, finish)) // stay put?
        {
            openNeighbors.Add(new Step(step.Pos, minute));
        }

        return openNeighbors;
    }

    private bool OpenNeighbor(Dictionary<Pos, int> blizzards, Pos pos, Pos start, Pos finish)
    {
        bool openNeighbor = false;
        if (blizzards.ContainsKey(pos) == false)
        {
            openNeighbor = (pos == finish) || (pos == start) || 
                           (pos.row >= _boundary.minRow && pos.row <= _boundary.maxRow && pos.col >= _boundary.minCol && pos.col <= _boundary.maxCol);
        }
        return openNeighbor;
    }

    private void DumpBlizzards(int minute)
    {
        Dictionary<Pos, int> state = _blizzardStates[minute % _lcm];
        Console.WriteLine("Minute: {0}", minute);
        for (int col = _boundary.minCol - 1; col <= _boundary.maxCol + 1; col++)
        {
            Console.Write('#');
        }
        Console.WriteLine();
        for (int row = _boundary.minRow; row <= _boundary.maxRow; row++)
        {
            Console.Write('#');
            for (int col = _boundary.minCol; col <= _boundary.maxCol; col++)
            {
                int mask;
                int count = 0;
                if (state.TryGetValue(new Pos(row, col), out mask))
                {
                    // count the blizzards at this location
                    int tempMask = mask;
                    for (int i = 0; i < 4; i++)
                    {
                        count += (tempMask & 1);
                        tempMask >>= 1;
                    }
                }
                if (count > 0)
                {
                    char c = '?';
                    if (count > 1)
                    {
                        c = (char)('0' + count);
                    }
                    else
                    {
                        switch (mask)
                        {
                            case UP: c = '^'; break;
                            case DOWN: c = 'v'; break;
                            case LEFT: c = '<'; break;
                            case RIGHT: c = '>'; break;
                        }
                    }
                    Console.Write(c);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine('#');
        }
        for (int col = _boundary.minCol - 1; col <= _boundary.maxCol + 1; col++)
        {
            Console.Write('#');
        }
        Console.WriteLine();
        Console.WriteLine();
    }


    public int CalcLCM(int a, int b) //method for finding LCM with parameters a and b
    {
        int num1;
        int num2;
        if (a > b)
        {
            num1 = a;
            num2 = b;
        }
        else
        {
            num1 = b;
            num2 = a;
        }

        for (int i = 1; i <= num2; i++)
        {
            if ((num1 * i) % num2 == 0)
            {
                return i * num1;
            }
        }
        return num2;
    }


}
