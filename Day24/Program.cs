// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Data;
using System.Reflection.Metadata.Ecma335;

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
    public record Boundary(int minRow, int maxRow, int minCol, int maxCol);
    public Boundary _boundary;
    public List<Blizzard> _blizzardsAtStart = new();
    public Dictionary<int, List<Blizzard>> _blizzardStates = new();
    public int _lcm = 0;
    public Pos _start = new Pos(0, 0);
    public Pos _finish = new Pos(0, 0);
    public Pos _target = new Pos(0, 0);

     public DayClass()
    {
         LoadData();
        //DumpBlizzards(0);
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
        Queue<Step> activeSteps = new(); 
        HashSet<Step> visited = new();

        activeSteps.Enqueue(checkStep);

        while (activeSteps.Any())
        {
            checkStep = activeSteps.Dequeue();

            //Console.WriteLine(string.Format("{0}, visited.Count: {1}, activeSteps.Count: {2}", checkStep.ToString(), visited.Count, activeSteps.Count)); ;
            if (checkStep.Pos == finish)
            {
                return checkStep; // destination!
            }

            if (visited.Contains(checkStep))
            {
                continue;
            }

            visited.Add(checkStep);

            List<Step> openNeighbors = OpenNeighbors(checkStep, start, finish);

            foreach (Step neighborStep in openNeighbors)
            {
                activeSteps.Enqueue(neighborStep);
            }
        }

        return new Step(new Pos(0, 0), -1); // not found :-(
    }

    private void LoadData()
    {
        string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt");
        _boundary = new Boundary(1, lines.Length - 2, 1, lines[0].Length - 2);
        List<Blizzard> currentState = new();
        for (int row = _boundary.minRow; row <= _boundary.maxRow; row++)
        {
            for (int col = _boundary.minCol; col <= _boundary.maxCol; col++)
            {
                if (lines[row][col] != '.')
                {
                   currentState.Add(new Blizzard(lines[row][col], new TimePos(0, new Pos(row, col))));
                }
            }
        }

        // get LCM of the boundaries - state will reach initial state at that point
        _lcm = CalcLCM(_boundary.maxRow, _boundary.maxCol);
        _blizzardStates[0] = currentState;
        //DumpBlizzards(0);
        // cache all blizzard states by minute
        for (int minute = 1; minute < _lcm; minute++)
        {
            List<Blizzard> newState = new();
            for (int bliz = 0; bliz < currentState.Count; bliz++)
            {
                newState.Add(currentState[bliz].Move(_boundary));
            }
            _blizzardStates[minute] = newState;
            //DumpBlizzards(minute);
            currentState = newState;
        }
    }

    public record Pos(int row, int col)
    {
        public Pos MoveTo(int row, int col)
        {
            return new Pos(row, col);
        }

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
        public TimePos MoveTo(Pos newPos)
        {
            return new TimePos(time + 1, pos.MoveTo(newPos.row, newPos.col));
        }

        public override string ToString()
        {
            return string.Format("time: {0}, {1}", time, pos);
        }
    }

    public record Blizzard(char direction, TimePos timePos)
    {
        public Blizzard Move(Boundary boundary)
        {
            int temp;

            switch (direction)
            {
                case '>':
                    temp = timePos.pos.col + 1;
                    temp = temp > boundary.maxCol ? boundary.minCol : temp;
                    return new Blizzard(direction, timePos.MoveTo(timePos.pos.MoveTo(timePos.pos.row, temp)));
                case '<':
                    temp = timePos.pos.col - 1;
                    temp = temp < boundary.minCol ? boundary.maxCol : temp;
                    return new Blizzard(direction, timePos.MoveTo(timePos.pos.MoveTo(timePos.pos.row, temp)));
                case '^':
                    temp = timePos.pos.row - 1;
                    temp = temp < boundary.minRow ? boundary.maxRow : temp;
                    return new Blizzard(direction, timePos.MoveTo(timePos.pos.MoveTo(temp, timePos.pos.col)));
                default: // case 'v':
                    temp = timePos.pos.row + 1;
                    temp = temp > boundary.maxRow ? boundary.minRow : temp;
                    return new Blizzard(direction, timePos.MoveTo(timePos.pos.MoveTo(temp, timePos.pos.col)));
            }
        }
        public override string ToString()
        {
            return string.Format("direction: {0}, {1}", direction, timePos);
        }
    }
    public class Step
    {
        public static Pos target = new Pos(0, 0);

        public Step(Pos pos, int minute)
        {
            Pos = pos;
            Minute = minute;
            Distance = Math.Abs(target.row - Pos.row) + Math.Abs(target.col - Pos.col);
            TPos = new TimePos(minute, pos);
        }
        public TimePos TPos { get; set; }
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
        List<Blizzard> blizzards = _blizzardStates[minute % _lcm];

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

    private bool OpenNeighbor(List<Blizzard> blizzards, Pos pos, Pos start, Pos finish)
    {
        bool openNeighbor = false;
        if (blizzards.FirstOrDefault(b => b.timePos.pos == pos) == null)
        {
            openNeighbor = (pos == finish) || (pos == start) || (pos.row >= _boundary.minRow && pos.row <= _boundary.maxRow && pos.col >= _boundary.minCol && pos.col <= _boundary.maxCol);
        }
        return openNeighbor;
    }

    private void DumpBlizzards(int minute)
    {
        List<Blizzard> state = _blizzardStates[minute % _lcm];
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
                var blizzards = state.Where(b => b.timePos.pos.row == row && b.timePos.pos.col == col);
                int count = blizzards.Count();
                if (count > 0)
                {
                    char c = count > 1 ? (char)('0' + count) : blizzards.First().direction;
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
        int num2;                         //taking input from user by using num1 and num2 variables
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
