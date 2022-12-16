// See https://aka.ms/new-console-template for more information
using Day15;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public record Point(long X, long Y);

internal class DayClass
{
    List<SBPair> _sbPairs = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {

        long minX = long.MaxValue;
        long maxX = long.MinValue;
        long targetRow = 2_000_000;

        foreach (SBPair pair in _sbPairs)
        {
            long minx;
            long maxx;
            bool isValid;

            (minx, maxx, isValid) = pair.MinMaxForY(targetRow);
            if (isValid)
            {
                minX = Math.Min(minX, minx);
                maxX = Math.Max(maxX, maxx);
            }
        }

        Console.WriteLine("Part1: {0}", maxX - minX);
    }

    public void Part2()
    {
        long rslt = 0;
        long minx;
        long maxx;

        List<(long min, long max)> rowPairs = new();
        bool found = false;

        // count down, assuming that the fault will be closer to the bottom than the top (just sayin')
        for (int row = 4_000_000; !found && row >= 0; row--)
        {
            rowPairs.Clear();
            foreach (SBPair pair in _sbPairs)
            {
                bool isValid;

                (minx, maxx, isValid) = pair.MinMaxForY(row);
                if (isValid)
                {
                    rowPairs.Add((minx, maxx));
                }
            }

            // sort the pairs by the minimum x
            // the gap will appear on a line where the min of a min/max pair is greater than the max for the line so far
            rowPairs.Sort((rec1, rec2) => (int)(rec1.min - rec2.min));
            minx = rowPairs[0].min;
            maxx = rowPairs[0].max;
            foreach ((long min, long max) rec in rowPairs)
            {
                if (rec.min > maxx)
                {
                    // we've found the gap! 
                    rslt = (maxx + 1) * 4_000_000 + row;
                    found = true;
                    break;
                } else if (rec.max >= maxx)
                {
                    maxx = rec.max;
                }
            }
        }

        Console.WriteLine("Part2: {0}", rslt);
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
                string[] nums = line.Replace("Sensor at x=", "").Replace(" y=", "").Replace(": closest beacon is at x=", ",").Replace(" ", "").Split(',');
                _sbPairs.Add(new SBPair(new Point(long.Parse(nums[0]), long.Parse(nums[1])), new Point(long.Parse(nums[2]), long.Parse(nums[3]))));
            }

            file.Close();
        }
    }

}
