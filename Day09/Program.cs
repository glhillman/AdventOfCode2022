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

internal class Point
{
    public Point(int id)
    {
        Id = id;
        Row = 0;
        Col = 0;
    }

    public int Id { get; private set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public override string ToString()
    {
        return string.Format("Id: {0}, Row:{1}, Col:{2}", Id, Row, Col);
    }
}

internal class DayClass
{
    List<(char dir, int dis)> _moves = new List<(char dir, int dis)> ();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        Console.WriteLine("Part1: {0}", MoveKnots(2));
    }

    public void Part2()
    {
        Console.WriteLine("Part2: {0}", MoveKnots(10));
    }


    private int MoveKnots(int nKnots)
    {
        HashSet<(int row, int col)> tailSpots = new HashSet<(int row, int col)>();

        List<Point> points = new List<Point>();
        for (int i = 0; i < nKnots; i++)
        {
            points.Add(new Point(i));
        }


        foreach (var move in _moves)
        {
            for (int i = 0; i < move.dis; i++)
            {
                MoveHead(move.dir, points[0]);
                for (int j = 0; j < points.Count - 1; j++)
                {
                    if (CheckProximity(points[j], points[j + 1]) == false)
                    {
                        break;
                    }
                }
                tailSpots.Add((points[nKnots-1].Row, points[nKnots-1].Col));
            }
        }

        return tailSpots.Count;
    }

    private void MoveHead(char dir, Point head)
    {
        switch (dir)
        {
            case 'U': head.Row--; break;
            case 'D': head.Row++; break;
            case 'L': head.Col--; break;
            case 'R': head.Col++; break;
        }
    }

    private bool CheckProximity(Point anchor, Point trailing)
    {
        bool trailingChanged = false;

        // check for anchor directly two steps up, down, left, or right of trailing
        if (anchor.Col == trailing.Col && Math.Abs(anchor.Row - trailing.Row) == 2)
        {
            int step = anchor.Row - trailing.Row;
            trailing.Row += step > 0 ? step - 1 : step + 1;
            trailingChanged = true;
        }
        else if (anchor.Row == trailing.Row && Math.Abs(anchor.Col - trailing.Col) == 2)
        {
            int step = anchor.Col - trailing.Col;
            trailing.Col += step > 0 ? step - 1 : step + 1;
            trailingChanged = true;
        }
        // check for anchor & trailing offset on a diagonal
        else if (Math.Abs(anchor.Row - trailing.Row) == 2 || Math.Abs(anchor.Col - trailing.Col) == 2)
        {
            int step = anchor.Col - trailing.Col;
            if (Math.Abs(step) > 1)
            {
                step = step > 0 ? step - 1 : step + 1;
            }
            trailing.Col += step;
            step = anchor.Row - trailing.Row;
            if (Math.Abs(step) > 1)
            {
                step = step > 0 ? step - 1 : step + 1;
            }
            trailing.Row += step;
            trailingChanged = true;
        }

        return trailingChanged;
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                _moves.Add((parts[0][0], int.Parse(parts[1])));
            }

            file.Close();
        }
    }

}
