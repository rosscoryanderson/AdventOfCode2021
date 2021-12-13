using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day12 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var caveSystem = new CaveSystem(Input);

        var numPaths = caveSystem.GetNumberOfPaths();

        return numPaths.ToString();
    }

    public string Puzzle2()
    {
        var caveSystem = new CaveSystem(Input);

        var numPaths = caveSystem.GetNumberOfPaths(true);

        return numPaths.ToString();
    }
}