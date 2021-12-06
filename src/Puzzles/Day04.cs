using src.Base;
using src.Features;
using src.Util;

namespace src.Puzzles;

public class Day04 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var bingo = new Bingo(Input);

        var finalScore = bingo.GetFinalScoreForGame();
        
        return finalScore.ToString();
    }

    public string Puzzle2()
    {
        var bingo = new Bingo(Input);

        var finalScore = bingo.GetFinalScoreForLosingGame();
        
        return finalScore.ToString();
    }
}