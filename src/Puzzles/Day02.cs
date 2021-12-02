using src.Base;
using src.Util;

namespace src.Puzzles;

public class Day02 : Day, IPuzzle
{
    public string Puzzle1()
    {
        int depth = 0, horizontalPosition = 0;
        var submarine = new Submarine();

        foreach (var value in Input)
        {
            (depth, horizontalPosition) = submarine.Move(value);
        }

        var result = depth * horizontalPosition;

        return result.ToString();
    }

    public string Puzzle2()
    {
        throw new NotImplementedException();
    }
}