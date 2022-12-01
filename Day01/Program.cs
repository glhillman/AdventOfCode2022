// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;

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
    List<long> _elf = new List<long>();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {

        long rslt = _elf.Max();

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        _elf.Sort();
        int len = _elf.Count - 1;
        long rslt = _elf[len] + _elf[len-1] + _elf[len-2];

        Console.WriteLine("Part2: {0}", rslt);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            long elf = 0;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    elf += long.Parse(line);
                }
                else
                {
                    _elf.Add(elf);
                    elf = 0;
                }
            }
            _elf.Add(elf);

            file.Close();
        }
    }

}
