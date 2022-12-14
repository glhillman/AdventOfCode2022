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
    int _NRows;
    int _NCols;
    Point _startPoint;
    Point _targetPoint;
    char[,] _heightMap;
    Dictionary<Point, List<Point>> _neighbors = new();
    

    public record Point(int col, int row);


    public DayClass()
    {
    }

    public void Part1()
    {
        LoadData();
        _neighbors = CacheNeighbors(_heightMap);
        DumpNeighbors();
        Dictionary<Point, int> distance = new();
        distance[_startPoint] = 0;
        int pathDistance = Search(_startPoint, _targetPoint, distance);

        Console.WriteLine("Part1: {0}", pathDistance);
    }

    public void Part2()
    {
        LoadData();

        long rslt = 0;

        Console.WriteLine("Part2: {0}", rslt);
    }

    public int Search(Point start, Point target, Dictionary<Point, int> distance)
    {
        HashSet<Point> visited = new();
        Queue<Point> queue = new();

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Point vertex = queue.Dequeue();
            if (visited.Contains(vertex) == false)
            {
                visited.Add(vertex);

                if (vertex == target)
                {
                    return distance[target];
                }

                foreach (Point neighbor in _neighbors[vertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        distance[neighbor] = distance[vertex] + 1;
                    }
                }
            }
        }

        return 0;
    }

    private void DumpNeighbors()
    {
        for (int row = 0; row < _NRows; row++)
        {
            for (int col = 0; col < _NCols; col++)
            {
                Point p = new Point(col, row);
                Console.Write(_neighbors[p].Count);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
    public Dictionary<Point, List<Point>> CacheNeighbors(char[,] grid)
    {
        Dictionary<Point, List<Point>> allNeighbors = new();
        for (int row = 0; row < _NRows; row++)
        {
            for (int col = 0; col < _NCols; col++)
            {
                List<Point> neighbors;

                if (grid[col, row] == 'S')
                {
                    neighbors = FindValidNeighbors(grid, col, row, 'a');
                    _startPoint = new Point(col, row);
                }
                else if (grid[col, row] == 'E')
                {
                    neighbors = FindValidNeighbors(grid, col, row, 'z');
                    _targetPoint = new Point(col, row);
                }
                else
                {
                    neighbors = FindValidNeighbors(grid, col, row, null);
                }
                allNeighbors[new Point(col, row)] = neighbors;
            }
        }
        return allNeighbors;
    }

    private List<Point> FindValidNeighbors(char[,] graph, int col, int row, char? anchorHeight)
    {
        List<Point> neighbors = new();
        if (anchorHeight == null)
        {
            anchorHeight = graph[col, row];
        }
        // check up, down, left, right for valid neighbor (no more than existing height + 1
        if (row > 0 && graph[col, row - 1] <= anchorHeight + 1) // up
        {
            neighbors.Add(new Point(col, row - 1));
        }
        if (row < _NRows - 1 && graph[col, row + 1] <= anchorHeight + 1) // down
        {
            neighbors.Add(new Point(col, row + 1));
        }
        if (col > 0 && graph[col - 1, row] <= anchorHeight + 1) // left
        {
            neighbors.Add(new Point(col - 1, row));
        }
        if (col < _NCols - 1 && graph[col + 1, row] <= anchorHeight + 1) // right
        {
            neighbors.Add(new Point(col + 1, row));
        }

        return neighbors;
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\test.txt";

        if (File.Exists(inputFile))
        {
            StreamReader file = new StreamReader(inputFile);
            string[] lines = file.ReadToEnd().Split("\r\n");
            _NRows = lines.Count();
            _NCols = lines[0].Length;
            _heightMap = new char[_NCols, _NRows];

            for (int r = 0; r < _NRows; r++)
            {
                for (int c = 0; c < _NCols; c++)
                {
                    _heightMap[c, r] = lines[r][c];
                }
            }
            
            file.Close();
        }
    }
}
