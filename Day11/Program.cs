// See https://aka.ms/new-console-template for more information
using Day11;
using System.Security.Cryptography.X509Certificates;

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
    public List<Monkey> _monkeys;
    public DayClass()
    {
    }

    public void Part1()
    {
        LoadData();
        Console.WriteLine("Part1: {0}", CalculateMonkeyBusiness(3, true, 20));
    }

    public void Part2()
    {
        LoadData();
        // create least common multiple for reducer
        long reducer = 1;
        foreach (Monkey monkey in _monkeys)
        {
            reducer *= monkey.Divisor;
        }
        Console.WriteLine("Part2: {0}", CalculateMonkeyBusiness(reducer, false, 10_000));
    }

    private long CalculateMonkeyBusiness(long reducer, bool reduceByDivision, int nRounds)
    {
        Monkey.Reducer = reducer;

        long[] inspectionCount = new long[_monkeys.Count];

        for (int i = 0; i < nRounds; i++)
        {
            foreach (Monkey monkey in _monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    long newValue = 0;

                    if (monkey.EvaluateNextItem(ref newValue, reduceByDivision) == true)
                    {
                        _monkeys[monkey.TrueTarget].AddItem(newValue);
                    }
                    else
                    {
                        _monkeys[monkey.FalseTarget].AddItem(newValue);
                    }

                    inspectionCount[monkey.Id]++;
                }
            }
        }

        Array.Sort(inspectionCount);

        int nItems = inspectionCount.Length;

        return inspectionCount[nItems - 1] * inspectionCount[nItems - 2];
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            _monkeys= new List<Monkey>();

            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                int id = line[7] - '0';
                line = file.ReadLine();
                string items = line[18..];
                line = file.ReadLine();
                string operations = line[19..];
                line = file.ReadLine();
                long divisor = long.Parse(line[20..]);
                line = file.ReadLine();
                int trueTarget = int.Parse(line.Substring(line.Length - 2));
                line = file.ReadLine();
                int falseTarget = int.Parse(line.Substring(line.Length - 2));
                file.ReadLine();

                _monkeys.Add(new Monkey(id, operations, divisor, trueTarget, falseTarget, items));
            }

            file.Close();
        }
    }

}
