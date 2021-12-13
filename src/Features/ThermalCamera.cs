using src.Domain;

namespace src.Features;

public class ThermalCamera
{
    private HashSet<Coordinate> _points;
    private List<Coordinate> _folds;

    public ThermalCamera(List<string> input)
    {
        _points = new HashSet<Coordinate>();
        _folds = new List<Coordinate>();
        PopulatePointsAndFolds(input);
    }

    public int PerformFold(int numFolds = 1)
    {
        for (int i = 0; i < numFolds; i++)
        {
            PerformFold(_folds[i]);
        }

        return _points.Count;
    }

    public void DisplayMessage()
    {
        PerformFold(_folds.Count);
        
        var height = _points.OrderByDescending(p => p.Y).First().Y + 1;
        var width = _points.OrderByDescending(p => p.X).First().X +1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_points.Contains(new Coordinate(x, y)))
                {
                    Console.Write(" X ");
                }
                else
                {
                    Console.Write("   ");
                }
            }
            Console.Write("\n");
        }
    }

    private void PerformFold(Coordinate fold)
    {
        if (fold.X > 0)
        {
            var existingPoints = _points.Where(p => p.X < fold.X);
            var pointsToFold = _points.Where(p => p.X > fold.X);
            _points = new HashSet<Coordinate>(existingPoints);
            
            foreach (var point in pointsToFold)
            {
                var distance = fold.X - point.X;
                var foldedCoordinate = new Coordinate(fold.X + distance, point.Y);
                _points.Add(foldedCoordinate);
            }
        }
        else
        {
            var existingPoints = _points.Where(p => p.Y < fold.Y);
            var pointsToFold = _points.Where(p => p.Y > fold.Y);
            _points = new HashSet<Coordinate>(existingPoints);
            
            foreach (var point in pointsToFold)
            {
                var distance = fold.Y - point.Y;
                var foldedCoordinate = new Coordinate(point.X, fold.Y + distance);
                _points.Add(foldedCoordinate);
            }
        }

        
    }

    private void PopulatePointsAndFolds(List<string> input)
    {
        foreach (var line in input.Where(line => !string.IsNullOrEmpty(line)))
        {
            AddFold(line);
            AddCoordinate(line);
        }
    }

    private void AddFold(string line)
    {
        const string phraseToRemove = "fold along ";

        if (!line.StartsWith("fold")) return;
        
        var values = line
            .Remove(0, phraseToRemove.Length)
            .Split('=');

        var value = int.Parse(values[1]);

        var coords = values[0] == "x"
            ? new Coordinate(value, 0)
            : new Coordinate(0, value);
            
        _folds.Add(coords);
    }

    private void AddCoordinate(string line)
    {
        if (line.StartsWith("fold")) return;
        
        var points = line.Split(',');
        var x = int.Parse(points[0]);
        var y = int.Parse(points[1]);

        var coords = new Coordinate(x, y);

        _points.Add(coords);
    }
}