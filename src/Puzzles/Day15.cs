using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day15 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var pathMapper = new PathMapper(Input);

        var shortestPath = pathMapper.FindShortestPath();
        
        return shortestPath.ToString();
    }

    public string Puzzle2()
    {
        var pathMapper = new PathMapper(Input);
        
        pathMapper.ExpandGrid();

        var shortestPath = pathMapper.FindShortestPath();
        
        return shortestPath.ToString();
    }
}