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
    internal enum Instruction
    {
        addx,
        noop
    };

    List<(Instruction instruction, int value)> _instructions = new List<(Instruction instruction, int value)>();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int cycle = 1;
        int xReg = 1;
        int sum = 0;

        foreach ((Instruction instruction, int value) inst in _instructions)
        {
            switch (inst.instruction)
            {
                case Instruction.addx:
                    sum += CheckCycleForSignalStrength(cycle, xReg);
                    cycle++;
                    sum += CheckCycleForSignalStrength(cycle, xReg);
                    cycle++;
                    xReg += inst.value;
                    break;
                case Instruction.noop:
                    sum += CheckCycleForSignalStrength(cycle, xReg);
                    cycle++;
                    break;
            }
        }

        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        char[] pixels;
        int cycle = 1;
        int xReg = 1;

        pixels = InitPixels();

        foreach ((Instruction instruction, int value) inst in _instructions)
        {
            switch (inst.instruction)
            {
                case Instruction.addx:
                    SetPixel(cycle, xReg, ref pixels);
                    cycle++;
                    SetPixel(cycle, xReg, ref pixels);
                    cycle++;
                    xReg += inst.value;
                    break;
                case Instruction.noop:
                    SetPixel(cycle, xReg, ref pixels);
                    cycle++;
                    break;
            }
        }
        SetPixel(cycle, xReg, ref pixels);
    }

    private void SetPixel(int cycle, int cursor, ref char[] pixels)
    {
        int modCycle = (cycle % 40) - 1;
        if (modCycle == cursor - 1 || modCycle == cursor || modCycle == cursor + 1)
        {
            if (modCycle >= 0)
            {
                pixels[modCycle] = '#';
            }
        }
        if (modCycle == 0)
        {
            string output = new string(pixels);
            Console.WriteLine(output);
            pixels = InitPixels();
        }
    }
    private int CheckCycleForSignalStrength(int cycle, int x)
    {
        int signalStrength = 0;

        switch (cycle)
        {
            case 20:
            case 60:
            case 100:
            case 140:
            case 180:
            case 220:
                signalStrength = cycle * x;
                break;
            default:
                break;
        }

        return signalStrength;
    }

    private char[] InitPixels()
    {
        char[] pixels = new char[40];
        for (int i = 0; i < 40; i++)
        {
            pixels[i] = ' ';
        }
        return pixels;
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
                string[] split = line.Split(' ');
                if (split[0] == "addx")
                {
                    _instructions.Add((Instruction.addx, int.Parse(split[1])));
                }
                else
                {
                    _instructions.Add((Instruction.noop, 0));
                }
            }

            file.Close();
        }
    }

}
