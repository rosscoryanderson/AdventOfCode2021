namespace src.Features;

public class HydrothermalVentReader
{
    private record Coordinate(int x, int y);

    private readonly List<string> _input;
    private Dictionary<Coordinate, int> _hydrothermalVents;
    

    public HydrothermalVentReader(List<string> input)
    {
        _hydrothermalVents = new Dictionary<Coordinate, int>();
        _input = input;
    }
    
    public int CalculateDangerousAreas()
    {
        ParseSteamVentReadings(_input, PopulatePointsStraightLines);
        return CalculateOverlappingPoints();
    }
    
    public int CalculateDangerousAreasDiagonal()
    {
        ParseSteamVentReadings(_input, PopulatePointsDiagonalLines);
        return CalculateOverlappingPoints();
    }

    private int CalculateOverlappingPoints()
    {
        return _hydrothermalVents
            .Where(vent => vent.Value > 1)
            .ToList()
            .Count;
    }

    private void ParseSteamVentReadings(List<string> input, Action<Coordinate, Coordinate> populatePoints)
    {
        foreach (var value in input)
        {
            var readings = value.Split(" -> ");

            var firstCoordinate = CreateCoordinate(readings[0]);
            var secondCoordinate = CreateCoordinate(readings[1]);

            populatePoints(firstCoordinate, secondCoordinate);
        }
    }

    private void PopulatePointsStraightLines(Coordinate firstCoordinate, Coordinate secondCoordinate)
    {
        var lineMapped = false;

        lineMapped = PopulateHorizontalLines(firstCoordinate, secondCoordinate) || lineMapped;
        lineMapped = PopulateVerticalLines(firstCoordinate, secondCoordinate) || lineMapped;

        if (lineMapped)
        {
            UpsertCoordinate(firstCoordinate);
            UpsertCoordinate(secondCoordinate);
        }
    }

    private void PopulatePointsDiagonalLines(Coordinate firstCoordinate, Coordinate secondCoordinate)
    {
        var lineMapped = false;

        lineMapped = PopulateHorizontalLines(firstCoordinate, secondCoordinate) || lineMapped;
        lineMapped = PopulateVerticalLines(firstCoordinate, secondCoordinate) || lineMapped;
        lineMapped = PopulateDiagonalLines(firstCoordinate, secondCoordinate) || lineMapped;

        if (lineMapped)
        {
            UpsertCoordinate(firstCoordinate);
            UpsertCoordinate(secondCoordinate);
        }
    }

    private bool PopulateHorizontalLines(Coordinate firstCoordinate, Coordinate secondCoordinate)
    {
        if (firstCoordinate.y != secondCoordinate.y) return false;
        
        var lowPoint = firstCoordinate.x > secondCoordinate.x ? secondCoordinate.x : firstCoordinate.x;
        var highPoint = firstCoordinate.x < secondCoordinate.x ? secondCoordinate.x : firstCoordinate.x;
        
        for (var x = lowPoint + 1; x < highPoint; x++)
        {
            var pathCoordinate = new Coordinate(x, firstCoordinate.y);
            UpsertCoordinate(pathCoordinate);
        }

        return true;
    }
    
    private bool PopulateVerticalLines(Coordinate firstCoordinate, Coordinate secondCoordinate)
    {
        if (firstCoordinate.x != secondCoordinate.x) return false;
        
        var lowPoint = firstCoordinate.y > secondCoordinate.y ? secondCoordinate.y : firstCoordinate.y;
        var highPoint = firstCoordinate.y < secondCoordinate.y ? secondCoordinate.y : firstCoordinate.y;
            
        for (var y = lowPoint + 1; y < highPoint; y++)
        {
            var pathCoordinate = new Coordinate(firstCoordinate.x, y);
            UpsertCoordinate(pathCoordinate);
        }

        return true;
    }
    
    private bool PopulateDiagonalLines(Coordinate firstCoordinate, Coordinate secondCoordinate)
    {
        var xOffset = Math.Abs(firstCoordinate.x - secondCoordinate.x);
        var yOffset = Math.Abs(firstCoordinate.y - secondCoordinate.y);
        
        if (xOffset != yOffset) return false;

        var xOffSetMultiplier = firstCoordinate.x < secondCoordinate.x ? 1 : -1;
        var yOffSetMultiplier = firstCoordinate.y < secondCoordinate.y ? 1 : -1;
        
        for (var i = 1; i < xOffset; i++)
        {
            var x = firstCoordinate.x + (i * xOffSetMultiplier);
            var y = firstCoordinate.y + (i * yOffSetMultiplier);
            
            var pathCoordinate = new Coordinate(x, y);
            UpsertCoordinate(pathCoordinate);
        }

        return true;
    }

    private void UpsertCoordinate(Coordinate coordinate)
    {
        if (_hydrothermalVents.ContainsKey(coordinate))
        {
            _hydrothermalVents[coordinate]++;
        }
        else
        {
            _hydrothermalVents.Add(coordinate, 1);
        }
    }

    private Coordinate CreateCoordinate(string reading)
    {
        var values = reading.Split(',');
        return new Coordinate(int.Parse(values[0]), int.Parse(values[1]));
    }
}