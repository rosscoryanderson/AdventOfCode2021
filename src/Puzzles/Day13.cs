using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day13 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var thermalCamera = new ThermalCamera(Input);

        var foldedPoints = thermalCamera.PerformFold();

        return foldedPoints.ToString();
    }

    public string Puzzle2()
    {
        var thermalCamera = new ThermalCamera(Input);

        thermalCamera.DisplayMessage();

        return string.Empty;
    }
}