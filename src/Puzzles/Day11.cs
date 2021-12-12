using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day11 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var jumboOctopus = new JumboOctopus(Input);

        var numFlashes = jumboOctopus.SimulateSteps();

        return numFlashes.ToString();
    }

    public string Puzzle2()
    {
        var jumboOctopus = new JumboOctopus(Input);

        var step = jumboOctopus.Synchronise();

        return step.ToString();
    }
}