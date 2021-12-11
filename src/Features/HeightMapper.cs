using src.Domain;

namespace src.Features;

public class HeightMapper
{
    private List<string> _readings;
    private HashSet<Coordinate> _capturedPoints;

    public HeightMapper(List<string> input)
    {
        _readings = input;
        _capturedPoints = new HashSet<Coordinate>();
    }

    public int LowPoints()
    {
        var lowPointSum = 0;
        
        for (int y = 0; y < _readings.Count; y++)
        {
            for (int x = 0; x < _readings[y].Length; x++)
            {
                var currentValue = _readings[y][x];
                var north = y > 0 ? _readings[y - 1][x] : char.MaxValue;
                var east = x < _readings[y].Length - 1 ? _readings[y][x + 1] : char.MaxValue;
                var south = y < _readings.Count - 1 ? _readings[y + 1][x] : char.MaxValue;
                var west = x > 0 ? _readings[y][x - 1] : char.MaxValue;
                
                if(currentValue >= north) continue;
                if(currentValue >= east) continue;
                if(currentValue >= south) continue;
                if(currentValue >= west) continue;

                lowPointSum += int.Parse(currentValue.ToString()) + 1;
            }
        }

        return lowPointSum;
    }

    public int LargestBasins()
    {
        var basins = new List<int>();

        var currentCoordinate = new Coordinate(0, 0);

        while (currentCoordinate.Y < _readings.Count)
        {
            var basin = CheckAdjacentPoints(new HashSet<Coordinate>(), currentCoordinate);

            basins.Add(basin.Count);

            currentCoordinate = MoveCoordinates(currentCoordinate);
        }

        var orderedBasins = basins.OrderByDescending(x => x).ToList();

        return orderedBasins[0] * orderedBasins[1] * orderedBasins[2];
    }

    private HashSet<Coordinate> CheckAdjacentPoints(HashSet<Coordinate> points, Coordinate currentPoint)
    {
        var currentChar = Character(currentPoint);
        
        if (currentChar == '9' || _capturedPoints.Contains(currentPoint)) return points;

        _capturedPoints.Add(currentPoint);
        points.Add(currentPoint);

        var y = currentPoint.Y;
        var x = currentPoint.X;
        
        if (y > 0)
        {
            var north = CheckAdjacentPoints(points, new Coordinate(x, y - 1));
            points.UnionWith(north);
        }

        if (x < _readings[y].Length - 1)
        {
            var east = CheckAdjacentPoints(points, new Coordinate(x + 1, y));
            points.UnionWith(east);
        }

        if (y < _readings.Count - 1)
        {
            var south = CheckAdjacentPoints(points, new Coordinate(x, y + 1));
            points.UnionWith(south);
        }

        if (x > 0)
        {
            var west = CheckAdjacentPoints(points, new Coordinate(x - 1, y));
            points.UnionWith(west);
        }

        return points;
    }

    private char Character(Coordinate coordinate)
    {
        return _readings[coordinate.Y][coordinate.X];
    }

    private Coordinate MoveCoordinates(Coordinate coordinate)
    {
        var newCoordinate = coordinate.X < _readings[0].Length - 2
            ? new Coordinate(coordinate.X + 1, coordinate.Y)
            : new Coordinate(0, coordinate.Y + 1);

        if (_capturedPoints.Contains(newCoordinate)) 
            newCoordinate = MoveCoordinates(newCoordinate);

        return newCoordinate;
    }
}