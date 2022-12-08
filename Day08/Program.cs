// See https://aka.ms/new-console-template for more information
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
    int[,] _trees;
    int _size = 0;

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int visibleTrees = 0;

        for (int row = 1; row < _size-1; row++)
        {
            for (int col = 1; col < _size-1; col++)
            {
                visibleTrees += IsVisible(row, col) ? 1 : 0;
            }
        }
        visibleTrees += 2 * _size + 2 * (_size - 2);

        Console.WriteLine("Part1: {0}", visibleTrees);
    }

    public void Part2()
    {
        int scenicScore = int.MinValue;

        for (int row = 1; row < _size - 1; row++)
        {
            for (int col = 1; col < _size - 1; col++)
            {
                scenicScore = int.Max(scenicScore, ScenicScore(row, col));
            }
        }

        Console.WriteLine("Part2: {0}", scenicScore);
    }

    private bool IsVisible(int row, int col)
    {
        bool isVisible = true;
        int currHeight = _trees[row,col];

        // check the row on either side
        // left of col
        for (int testCol = 0; testCol < col && isVisible; testCol++)
        {
            isVisible = _trees[row, testCol] < currHeight;
        }
        // right of col
        if (isVisible == false)
        {
            isVisible = true;
            for (int testCol = col + 1; testCol < _size && isVisible; testCol++)
            {
                isVisible = _trees[row, testCol] < currHeight;
            }
        }

        if (isVisible == false)
        {
            isVisible = true;
            // check the col
            // above row
            for (int testRow = 0; testRow < row && isVisible; testRow++)
            {
                isVisible = _trees[testRow, col] < currHeight;
            }
        }
        if (isVisible == false)
        {
            isVisible = true;
            // below row
            for (int testRow = row+1; testRow < _size && isVisible; testRow++)
            {
                isVisible = _trees[testRow, col] < currHeight;
            }
        }

        return isVisible;
    }

    private int ScenicScore(int row, int col)
    {
        int score = 1;
        int tempScore;
        int testCol;
        int testRow;

        int currHeight = _trees[row, col];

        // check the row on either side
        // look left
        tempScore = 0;
        testCol = col - 1;
        do
        {
            if (testCol >= 0)
            {
                tempScore++;
            }
        } while (testCol >= 0 && _trees[row, testCol--] < currHeight);

        score *= tempScore;
        
        // look right
        tempScore = 0;
        testCol = col + 1;
        do
        {
            if (testCol < _size)
            {
                tempScore++;
            }
        } while (testCol < _size && _trees[row, testCol++] < currHeight);

        score *= tempScore;

        // look up
        tempScore = 0;
        testRow = row - 1;
        do
        {
            if (testRow >= 0)
            {
                tempScore++;
            }
        } while (testRow >= 0 && _trees[testRow--, col] < currHeight);

        score *= tempScore;

        // look down
        tempScore = 0;
        testRow = row + 1;
        do
        {
            if (testRow < _size)
            {
                tempScore++;
            }
        } while (testRow < _size && _trees[testRow++, col] < currHeight);

        return score * tempScore;
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            int row = 0;
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                if (_trees== null)
                {
                    _size = line.Length;
                    _trees = new int[_size,_size];
                }
                for (int col = 0; col < _size; col++)
                {
                    _trees[row, col] = line[col] - '1' + 1;
                }
                row++;
            }

            file.Close();
        }
    }

}
