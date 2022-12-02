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
    List<(char opponent, char me)> _moves = new List<(char opponent, char me)>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int totalScore = 0;

        foreach ((char opponent, char me) in _moves)
        {
            totalScore += Play1(opponent, me);
        }

        Console.WriteLine("Part1: {0}", totalScore);
    }

    public void Part2()
    {
        int totalScore = 0;

        foreach ((char opponent, char me) in _moves)
        {
            totalScore += Play2(opponent, me);
        }

        Console.WriteLine("Part2: {0}", totalScore);
    }

    private int Play1(char opponent, char me)
    {
        int score = 0;

        switch (opponent)
        {
            case 'R': // rock
                switch (me)
                {
                    case 'X':
                        score = 1 + 3; // rock == rock
                        break;
                    case 'Y':
                        score = 2 + 6; // paper covers rock
                        break;
                    case 'Z':
                        score = 3; // rock breaks scissors
                        break;
                }
                break;
            case 'P': // paper
                switch (me)
                {
                    case 'X':
                        score = 1; //paper covers rock
                        break;
                    case 'Y':
                        score = 2 + 3; // paper == paper 
                        break;
                    case 'Z':
                        score = 3 + 6; // scissors cuts paper
                        break;
                }
                break;
            case 'S': // scissors
                switch (me)
                {
                    case 'X':
                        score = 1 + 6; // rock breaks scissors
                        break;
                    case 'Y':
                        score = 2; // scissors cuts paper 
                        break;
                    case 'Z':
                        score = 3 + 3; // scissors == scissors
                        break;
                }
                break;
        }
        return score;
    }

    private int Play2(char opponent, char me)
    {
        int score = 0;

        switch (opponent)
        {
            case 'R': // rock
                switch (me)
                {
                    case 'X': // lose
                        score = 3; // rock breaks scissors
                        break;
                    case 'Y': // draw
                        score = 1 + 3; // rock == rock
                        break;
                    case 'Z': // win
                        score = 2 + 6; // paper covers rock
                        break;
                }
                break;
            case 'P': // paper
                switch (me)
                {
                    case 'X':
                        score = 1; //paper covers rock
                        break;
                    case 'Y':
                        score = 2 + 3; // paper == paper 
                        break;
                    case 'Z':
                        score = 3 + 6; // scissors cuts paper
                        break;
                }
                break;
            case 'S': // scissors
                switch (me)
                {
                    case 'X':
                        score = 2; // scissors cut paper
                        break;
                    case 'Y':
                        score = 3 + 3; // scissors  == scissors 
                        break;
                    case 'Z':
                        score = 1 + 6; // rock breaks scissors
                        break;
                }
                break;
        }
        return score;
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
                char opponent = line[0];
                char me = line[2];
                switch (opponent)
                {
                    case 'A': opponent = 'R'; break;
                    case 'B': opponent = 'P'; break;
                    case 'C': opponent = 'S'; break;
                }
                (char opponent, char me) rec = new (opponent, me);
                _moves.Add(rec);
            }

            file.Close();
        }
    }

}
