using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day07 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var crabSub = new CrabSubmarine(Input);

        var fuelUsed = crabSub.CalculateFuelUsage();

        return fuelUsed.ToString();
    }

    public string Puzzle2()
    {
        var crabSub = new CrabSubmarine(Input);

        var fuelUsed = crabSub.CalculateIncreasingFuelUsage();

        return fuelUsed.ToString();
    }
}