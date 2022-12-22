// See https://aka.ms/new-console-template for more information
using Day17;
using System.Text;
using System.Threading.Channels;

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
    string _pattern;
    int _patternIndex = 0;
    int _previousPatternIndex;
    int _shapeIndex = 0;

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        Console.WriteLine("Part1: {0}", DropRocks(2022)); ;
    }

    public void Part2()
    {
        (Chamber chamber, int firstRock) = DropRocksFindPattern(5000); // 5000 is arbitrary - function bails on first pattern detection
        List<(int, int)> last = chamber.Tops.Last().Value; // n rocks between pattern start
        int rocksPerPattern = last[1].Item1 - last[0].Item1;
        int patternLength = last[1].Item2 - last[0].Item2;
        
        
        // number of rocks that haven't been dropped
        long undroppedRocks = (1_000_000_000_000 + rocksPerPattern - firstRock);
        // number of patterns that will appear in the undropped Rocks
        long nPatterns = undroppedRocks / rocksPerPattern;
        long chamberHeightOfCalculatedPatterns = nPatterns * patternLength + last[0].Item2;
        long rocksToFinish = undroppedRocks % rocksPerPattern;
        long currentChamberTop = chamber.HighestRow;
        
        DropRemainingRocks(chamber, rocksToFinish);

        long diff = chamber.HighestRow - 1 - currentChamberTop;
        long heightAfterATrillionRocks = diff + chamberHeightOfCalculatedPatterns;

        Console.WriteLine("Part2: {0}", heightAfterATrillionRocks);
    }

    public long DropRocks(long nToDrop)
    {
        int rocks = 0;
        Chamber chamber = new();
        _patternIndex = 0;
        _shapeIndex = 0;
        Shape shape = GetNextShape(chamber);
        while (rocks < nToDrop)
        {
            shape.Shift(chamber, GetNextMove());
            shape.Drop(chamber);
            if (shape.Settled)
            {
                rocks++;
                shape = GetNextShape(chamber);
            }
        }

        return chamber.HighestRow - 1;
    }

    public (Chamber, int) DropRocksFindPattern(long nToDrop)
    {
        int rocks = 0;
        Chamber chamber = new();
        _patternIndex = 0;
        _shapeIndex = 0;
        bool patternFound = false;
        Shape shape = GetNextShape(chamber);
        while (patternFound == false && rocks < nToDrop)
        {
            shape.Shift(chamber, GetNextMove());
            shape.Drop(chamber);
            if (shape.Settled)
            {
                chamber.StoreSettledShape(rocks, _previousPatternIndex);
                if (chamber.Tops.Count > 0)
                {
                    var last = chamber.Tops.Last().Value;
                    if (last.Count > 1)
                    {
                        patternFound = true;
                    }
                }
                if (patternFound == false)
                {
                    rocks++;
                    shape = GetNextShape(chamber);
                }
            }
        }
        return (chamber, rocks);
    }

    public long DropRemainingRocks(Chamber chamber, long nToDrop)
    {
        int rocks = 0;
        Shape shape = GetNextShape(chamber);
        while (rocks < nToDrop)
        {
            shape.Shift(chamber, GetNextMove());
            shape.Drop(chamber);
            if (shape.Settled)
            {
                rocks++;
                shape = GetNextShape(chamber);
            }
        }

        return chamber.HighestRow - 1;
    }

    public char GetNextMove()
    {
        if (_patternIndex >= _pattern.Length)
        {
            _patternIndex = 0;
        }
        _previousPatternIndex = _patternIndex++;
        return _pattern[_previousPatternIndex];
    }

    public Shape GetNextShape(Chamber chamber)
    {
        switch (_shapeIndex++ % 5)
        {
            case 0: return new Bar(chamber);
            case 1: return new Plus(chamber);
            case 2: return new Comma(chamber);
            case 3: return new Tower(chamber);
            default: return new Block(chamber); // case 4
        }
    }

    public void DumpChamber(int nthRock, string title, Chamber chamber, Shape shape)
    {
        //return;
        Console.WriteLine(string.Format("Rock: {0}, {1}", nthRock, title));
        int highestRow = chamber.HighestRow - 1;

        if (shape.Settled == false)
        {
            int currRow = shape.BottomRow + shape.Height - 1;
            for (int row = shape.Height - 1 ; row >= 0; row--)
            {
                byte rowToDump = (byte)(shape.ShapeData[row] >> shape.Offset);
                if (currRow < chamber.HighestRow)
                {
                    rowToDump |= chamber.Rows[currRow].Row;
                    highestRow--;
                }
                currRow--;
                
                DumpRow(rowToDump);
            }
        }
        int usedRow = shape.BottomRow;
        while (usedRow > chamber.HighestRow)
        {
            DumpRow(0);
            usedRow--;
        }
        for (int row = highestRow; row > 0; row--)
        {
            DumpRow(chamber.Rows[row].Row);
        }
        Console.WriteLine("+-------+");
        Console.WriteLine();
    }

    public void DumpRow(byte row)
    {
        Console.Write("|");
        byte mask = 0b1000000;
       
        for (int col = 0; col < 7; col++)
        {
            byte temp = (byte)(row & mask);
            Console.Write(string.Format("{0}", temp == mask ? '#' : '.'));
            mask >>= 1;
        }
        Console.WriteLine("|");
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\test.txt";

        if (File.Exists(inputFile))
        {
            StreamReader file = new StreamReader(inputFile);
            _pattern = file.ReadLine();

            file.Close();
        }
    }

}
