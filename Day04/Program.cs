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

    List<((int low1, int high1), (int low2, int high2))> _sections = new List<((int low1, int high1), (int low2, int high2))>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int containsCount = 0;

        foreach (var section in _sections)
        {
            if (Contains(section.Item1, section.Item2))
            {
                containsCount++;
            }
        }

        Console.WriteLine("Part1: {0}", containsCount);
    }

    public void Part2()
    {
        int overlapsCount = 0;

        foreach (var section in _sections)
        {
            if (Overlaps(section.Item1, section.Item2))
            {
               overlapsCount++;
            }
        }

        Console.WriteLine("Part1: {0}", overlapsCount);
    }

    private bool Contains((int low1, int high1) item1, (int low2, int high2) item2)
    {
        return ((item2.low2 >= item1.low1 && item2.high2 <= item1.high1) ||
                (item1.low1 >= item2.low2 && item1.high1 <= item2.high2));
    }

    private bool Overlaps((int low1, int high1) item1, (int low2, int high2) item2)
    {
        return ((item2.low2 >= item1.low1 && item2.low2 <= item1.high1) ||
                (item1.low1 >= item2.low2 && item1.low1 <= item2.high2));
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
                int low1;
                int high1;
                int low2;
                int high2;

                string[] split = line.Split(',');
                string[] lowHigh = split[0].Split('-');
                low1 = int.Parse(lowHigh[0]);
                high1 = int.Parse(lowHigh[1]);
                lowHigh = split[1].Split('-');
                low2 = int.Parse(lowHigh[0]);
                high2 = int.Parse(lowHigh[1]);

                _sections.Add(((low1, high1), (low2, high2)));
            }

            file.Close();
        }
    }

}
