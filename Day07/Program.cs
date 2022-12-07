// See https://aka.ms/new-console-template for more information
using Day07;

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
    List<string> _input = new List<string>();
    List<Entry> _entries= new List<Entry>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        ParseInput();
        long size = 0;

        foreach (Entry entry in _entries)
        {
            if (entry.IsDir && entry.Size <= 100000)
            {
                size += entry.Size;
            }
        }

        Console.WriteLine("Part1: {0}", size);
    }

    public void Part2()
    {
        long freeSpace = 70000000 - _entries[0].Size;
        long requiredSpace = 30000000 - freeSpace;

        var bestChoice = _entries.FindAll(e => e.IsDir && e.Size >= requiredSpace).Min(e => e.Size);

        Console.WriteLine("Part2: {0}", bestChoice);
    }

    private void ParseInput()
    {
        Entry? currentEntry = null;
        int index = 0;

        foreach (string input in _input)
        {
            index++;
            if (index == 170)
            {
                int foo = 0;
            }
            string[] parts = input.Split(' ');
            switch (parts[0])
            {
                case "$":
                    switch(parts[1])
                    {
                        case "cd":
                            if (parts[2] == "..")
                            {
                                if (currentEntry.Parent != null)
                                {
                                    currentEntry = currentEntry.Parent;
                                }
                            }
                            else
                            {
                                // see if directory entry exists. If not, create it.
                                // there may be subdirs with same name, make sure we are checking inside the current dir
                                var entry = _entries.FirstOrDefault(e => e.Name == parts[2] && e.Parent == currentEntry);
                                {
                                    if (entry == null)
                                    {
                                        Entry newEntry = new Entry(parts[2], currentEntry, true);
                                        _entries.Add(newEntry);
                                        currentEntry = newEntry;
                                    }
                                    else
                                    {
                                        currentEntry = entry;
                                    }
                                }
                            }
                            break;
                        case "ls": // no processing necessary
                            break;
                    }
                    break;
                case "dir":
                    // see if directory entry exists. If not, create it.
                    var tempEntry = _entries.FirstOrDefault(e => e.Name == parts[1]);
                    {
                        if (tempEntry == null)
                        {
                            _entries.Add(new Entry(parts[1], currentEntry, true));
                        }
                    }
                    break;
                default: // file w/size
                    Entry fileEntry = new Entry(parts[1], currentEntry, false);
                    fileEntry.AddSize(long.Parse(parts[0]));
                    _entries.Add(fileEntry);
                    break;
            }
        }
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
                _input.Add(line);
            }

            file.Close();
        }
    }

}
