// See https://aka.ms/new-console-template for more information
using System.Text;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    string[] _snafuNumbers;
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long sum = 0;

        foreach (string snafu in _snafuNumbers)
        {
            sum += SnafuToLong(snafu);
        }

        string snafuSum = LongToSnafu(sum);

        Console.WriteLine("Part1: {0}", snafuSum);
    }
    private long SnafuToLong(string snafu)
    {
        long rslt = 0;

        foreach (char c in snafu)
        {
            rslt *= 5;
            switch (c)
            {
                case '0':
                    rslt += 0; // uh, += 0? included because it bugs me to leave it out...
                    break;
                case '1':
                    rslt += 1; 
                    break;
                case '2': 
                    rslt += 2; 
                    break;
                case '-': 
                    rslt += -1; 
                    break;
                case '=': 
                    rslt += -2; 
                    break;
            }
        }

        return rslt;
    }

    private string LongToSnafu(long num)
    {
        string snafu = "";

        while (num > 0)
        {
            switch (num % 5)
            {
                case 0: 
                    snafu = "0" + snafu; 
                    break;
                case 1: 
                    snafu = "1" + snafu; 
                    break;
                case 2: 
                    snafu = "2" + snafu; 
                    break;
                case 3: 
                    num += 5; 
                    snafu = "=" + snafu; 
                    break;
                case 4: 
                    num += 5; 
                    snafu = "-" + snafu; 
                    break;
            }

            num /= 5;
        }

        return snafu;
    }
    private void LoadData()
    {
        _snafuNumbers = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt");
    }

}
