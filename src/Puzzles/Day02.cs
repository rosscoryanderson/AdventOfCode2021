using src.Base;
using src.Util;

namespace src.Puzzles;

public class Day02 : Day, IPuzzle
{
    public string Puzzle1()
    {
        return CalculatePosition();
    }

    public string Puzzle2()
    {
        return CalculatePosition(true);
    }

    private string CalculatePosition(bool useAim = false)
    {
        int depth = 0, horizontalPosition = 0;
        var submarine = new Submarine(useAim);

        foreach (var value in Input)
        {
            (depth, horizontalPosition) = submarine.Move(value);
        }

        var result = depth * horizontalPosition;

        return result.ToString();
    }
}