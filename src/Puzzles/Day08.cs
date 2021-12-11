using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day08 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var digitDisplay = new DigitDisplay(Input);

        var easyDigits = digitDisplay.GetEasyDigits();

        return easyDigits.ToString();
    }

    public string Puzzle2()
    {
        var digitDisplay = new DigitDisplay(Input);

        var totalDigits = digitDisplay.GetTotalOutputDigits();

        return totalDigits.ToString();
    }
}