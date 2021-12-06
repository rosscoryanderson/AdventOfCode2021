namespace src.Base;

public class Day
{
    protected readonly List<string> Input;

    protected Day(string inputLocation = @"C:\Dev\aoc\aoc2021\src\Input\")
    {
        var file = $"{GetType().Name}.txt";

        Input = Util.FileReader.ReadFileToList($"{inputLocation}{file}");
    }
}