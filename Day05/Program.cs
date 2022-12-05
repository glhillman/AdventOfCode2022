// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
    List<Stack<char>> _stacks1 = new List<Stack<char>>();
    List<Stack<char>> _stacks2 = new List<Stack<char>>();
    List<(int, int, int)> _moves = new List<(int, int, int)> ();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        foreach ((int count, int from, int to) in _moves)
        {
            for (int i = 0; i < count; i++)
            {
                _stacks1[to].Push(_stacks1[from].Pop());
            }
        }

        Console.WriteLine("Part1: {0}", TopsToString(_stacks1));
    }

    public void Part2()
    {
        Stack<char> tempStack = new Stack<char>();

        foreach ((int count, int from, int to) in _moves)
        {
            for (int i = 0; i < count; i++)
            {
                tempStack.Push(_stacks2[from].Pop());
            }
            for (int i = 0; i < count; i++)
            {
                _stacks2[to].Push(tempStack.Pop());
            }
        }

        Console.WriteLine("Part2: {0}", TopsToString(_stacks2));
    }

    private string TopsToString(List<Stack<char>> stacks)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < stacks.Count; i++)
        {
            sb.Append(stacks[i].Peek());
        }
        return sb.ToString();
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            List<string> rawStrings = new List<string>();
            int stackCount = 0;
            while ((line = file.ReadLine()).Length != 0)
            {
                if (line[1] != '1')
                {
                    rawStrings.Add(line);
                }
                else
                {
                    stackCount = (line[line.Length - 2] - '1' + 1);
                    for (int i = 0; i < stackCount; i++)
                    {
                        _stacks1.Add(new Stack<char>());
                        _stacks2.Add(new Stack<char>());
                    }
                    for (int i = rawStrings.Count-1; i >= 0; i--)
                    {
                        string rawString = rawStrings[i];
                        int column = 0;
                        for (int offset = 1; offset < rawString.Length; offset += 4)
                        {
                            if (rawString[offset] != ' ')
                            {
                                _stacks1[column].Push(rawString[offset]);
                                _stacks2[column].Push(rawString[offset]);
                            }
                            column++;
                        }
                    }
                    // stacks are built - get the instructions
                    // throw away blank line
                }
            }

            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(' ');
                _moves.Add((int.Parse(parts[0]), int.Parse(parts[1]) - 1, int.Parse(parts[2]) - 1));
            }

            file.Close();
        }
    }

}
