// See https://aka.ms/new-console-template for more information
using Day21;
using System.Security.Cryptography;

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
    Dictionary<string, Monkey> _monkeys;
    Dictionary<string, double> _constants;
    string[] _lines;

    public DayClass()
    {
    }

    public void Part1()
    {
        LoadData();

        Monkey root = _monkeys["root"];
        double rootNum = 0;

        while (rootNum == 0)
        {
            foreach (Monkey monkey in _monkeys.Values)
            {
                if (monkey.Num1 == null && _constants.ContainsKey(monkey.Monkey1Name))
                {
                    monkey.Num1 = _constants[monkey.Monkey1Name]; 
                }
                if (monkey.Num2 == null && _constants.ContainsKey(monkey.Monkey2Name))
                {
                    monkey.Num2 = _constants[monkey.Monkey2Name];
                }
                if (monkey.Num1 != null && monkey.Num2 != null)
                {
                    _constants[monkey.Name] = DoMath(monkey.Num1.Value, monkey.Op, monkey.Num2.Value);
                    if (monkey.Name == "root")
                    {
                        rootNum = _constants[monkey.Name];
                    }
                    else
                    {
                        _monkeys.Remove(monkey.Name);
                    }
                }
            }
        }

        Console.WriteLine("Part1: {0}", rootNum);
    }

    public void Part2()
    {
        double minNum = long.MinValue;
        double maxNum = long.MaxValue;
        double mid;
        bool found = false;
        Monkey root;

        while (!found && minNum <= maxNum)
        {
            mid = (minNum + maxNum) / 2.0;
            //Console.WriteLine(string.Format("Min: {0}, Max: {1}, Mid: {2}", minNum, maxNum, mid));
            
            // seek humn number
            
            LoadData();
            root = _monkeys["root"];
            root.Op = '=';
            _constants["humn"] = mid;

            while (root.Num1 == null || root.Num2 == null)
            {
                foreach (Monkey monkey in _monkeys.Values)
                {
                    if (monkey.Num1 == null && _constants.ContainsKey(monkey.Monkey1Name))
                    {
                        monkey.Num1 = _constants[monkey.Monkey1Name];
                    }
                    if (monkey.Num2 == null && _constants.ContainsKey(monkey.Monkey2Name))
                    {
                        monkey.Num2 = _constants[monkey.Monkey2Name];
                    }
                    if (monkey.Num1 != null && monkey.Num2 != null)
                    {
                        _constants[monkey.Name] = DoMath(monkey.Num1.Value, monkey.Op, monkey.Num2.Value);
                        _monkeys.Remove(monkey.Name);
                    }
                }
            }
            
            double rootNum = _constants["root"];
            if (rootNum == 0.0)
            {
                found = true;
            }
            else
            {
                double humnNum = _constants["humn"];
                if (rootNum > 0)
                {
                    minNum = humnNum;
                }
                else
                {
                    maxNum = humnNum;
                }
            }
        }
        double rslt = _constants["humn"];
        Console.WriteLine("Part2: {0}", rslt);
    }

    public double DoMath(double value1, char op, double value2)
    {
        double rslt = 0;

        switch (op)
        {
            case '+': rslt = value1 + value2; break;
            case '-': rslt = value1 - value2; break;
            case '/': rslt = value1 / value2; break;
            case '*': rslt = value1 * value2; break;
            case '=': rslt = value1 - value2; break;
        }

        return rslt;
    }
    private void LoadData()
    {
        if (_lines == null)
        {
            _lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt");
        }

        _monkeys = new();
        _constants = new();
        foreach (string line in _lines)
        {
            string[] split = line.Split(':', ' ');
            if (split.Length == 5)
            {
                _monkeys[split[0]] = new Monkey(split[0], split[2], split[3], split[4]);
            }
            else
            {
                _constants[split[0]] = double.Parse(split[2]);
            }
        }
    }

}
