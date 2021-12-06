using src.Base;
using src.Features;
using src.Util;

namespace src.Puzzles;

public class Day03 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var submarine = new Submarine();

        var powerConsumption = submarine.CalculatePowerConsumption(Input);

        return powerConsumption.ToString();
    }

    public string Puzzle2()
    {
        var submarine = new Submarine();

        var lifeSupportRating = submarine.CalculateLifeSupportRating(Input);

        return lifeSupportRating.ToString();
    }
}