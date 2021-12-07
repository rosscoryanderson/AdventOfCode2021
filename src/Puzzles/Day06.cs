using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day06 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var lfc = new LanternFishCalculator(Input);

        var numberOfFish = lfc.CalculateGrowthRate(80);
        
        return numberOfFish.ToString();
    }

    public string Puzzle2()
    {
        var lfc = new LanternFishCalculator(Input);

        var numberOfFish = lfc.CalculateGrowthRate(256);
        
        return numberOfFish.ToString();
    }
}