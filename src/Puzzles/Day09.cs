using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day09 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var heightMapper = new HeightMapper(Input);

        var lowPoints = heightMapper.LowPoints();

        return lowPoints.ToString();
    }

    public string Puzzle2()
    {
        var heightMapper = new HeightMapper(Input);

        var largestBasins = heightMapper.LargestBasins();

        return largestBasins.ToString();
    }
}