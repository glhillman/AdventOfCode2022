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
    List<string> _rucksacks = new List<string> ();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int prioritySum = 0;
        foreach (string str in _rucksacks)
        {
            int len = str.Length / 2;

            var inter = str.Substring(0, len).Intersect(str.Substring(len));
            char chr = inter.First();
            prioritySum += char.IsLower(chr) ? chr - 'a' + 1 : chr - 'A' + 27;
        }

        Console.WriteLine("Part1: {0}", prioritySum);
    }

    public void Part2()
    {
        int prioritySum = 0;
        int count = _rucksacks.Count;
        for (int index = 0; index < count; index += 3)
        {
            var inter = _rucksacks[index].Intersect(_rucksacks[index+1]).Intersect(_rucksacks[index+2]);
            char chr = inter.First();
            prioritySum += char.IsLower(chr) ? chr - 'a' + 1 : chr - 'A' + 27;
        }

        Console.WriteLine("Part2: {0}", prioritySum);
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
                _rucksacks.Add(line);
            }

            file.Close();
        }
    }

}
