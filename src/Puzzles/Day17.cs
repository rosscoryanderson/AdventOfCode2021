using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day17 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var launcher = new ProbeLauncher(Input[0]);

        var y = launcher.LaunchTrickShot();
        
        return y.ToString();
    }

    public string Puzzle2()
    {
        var launcher = new ProbeLauncher(Input[0]);

        var collisions = launcher.GetAllLandings();
        
        return collisions.ToString();
    }
}