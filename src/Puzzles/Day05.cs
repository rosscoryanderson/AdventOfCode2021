using src.Base;
using src.Features;
using src.Util;

namespace src.Puzzles;

public class Day05 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var hvr = new HydrothermalVentReader(Input);

        var dangerousVents = hvr.CalculateDangerousAreas();

        return dangerousVents.ToString();
    }

    public string Puzzle2()
    {
        var hvr = new HydrothermalVentReader(Input);

        var dangerousVents = hvr.CalculateDangerousAreasDiagonal();

        return dangerousVents.ToString();
    }
}