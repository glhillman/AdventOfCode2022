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
    string _data;
    HashSet<char> _markers = new HashSet<char>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        Console.WriteLine("Part1: {0}", FindUniqueSequence(4));
    }

    public void Part2()
    {
        Console.WriteLine("Part2: {0}", FindUniqueSequence(14));
    }

    private int FindUniqueSequence(int sequenceSize)
    {
        int index = 0;
        bool found = false;
        _markers.Clear();

        while (!found)
        {
            for (int i = index; i < index + sequenceSize; i++)
            {
                if (_markers.Add(_data[i]) == false)
                {
                    index++;
                    _markers.Clear();
                    break;
                }
            }

            if (_markers.Count == sequenceSize)
            {
                found = true;
            }
        }

        return index + sequenceSize;
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            StreamReader file = new StreamReader(inputFile);
            _data = file.ReadLine();
            file.Close();
        }
    }

}
