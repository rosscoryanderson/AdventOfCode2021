using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day10 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var syntaxReader = new SyntaxReader(Input);

        var errorScore = syntaxReader.GetCorruptedPoints();

        return errorScore.ToString();
    }

    public string Puzzle2()
    {
        var syntaxReader = new SyntaxReader(Input);

        var autocompleteScore = syntaxReader.AutoComplete();

        return autocompleteScore.ToString();
    }
}