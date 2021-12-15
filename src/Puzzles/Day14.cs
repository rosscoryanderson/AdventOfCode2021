using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day14 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var poly = new Polymiser(Input);

        var result = poly.CalculateHighestLowestProduct();

        return result.ToString();
    }

    public string Puzzle2()
    {
        var poly = new Polymiser(Input);

        var result = poly.CalculateHighestLowestProduct(40);

        return result.ToString();
    }
}