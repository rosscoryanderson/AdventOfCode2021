namespace src.Util;

public class Submarine
{
    private int _depth;
    private int _horizontalPosition;

    private List<string> legalMoves = new List<string>
    {
        "forward",
        "down",
        "up"
    };

    public Submarine()
    {
        _depth = 0;
        _horizontalPosition = 0; 
    }

    public Submarine(int depth, int horizontalPosition)
    {
        _depth = depth;
        _horizontalPosition = horizontalPosition;
    }

    public (int depth, int horizontalPosition) Move(string input)
    {
        var (direction, distance) = ParseInput(input);

        if (!string.IsNullOrEmpty(direction))
            UpdatePosition(direction, distance);
            
        return (_depth, _horizontalPosition);
    }

    private (string direction, int distance) ParseInput(string input)
    {
        var pieces = input.Split(' ');
        
        if (pieces.Length >= 2 && legalMoves.Contains(pieces[0].ToLower()) && int.TryParse(pieces[1], out var distance))
        {
            return (pieces[0].ToLower(), distance);
        }

        return (string.Empty, 0);
    }

    private void UpdatePosition(string direction, int distance)
    {
        switch (direction)
        {
            case "forward":
                _horizontalPosition += distance;
                break;
            case "up":
                _depth -= distance;
                break;
            case "down":
                _depth += distance;
                break;
        }
    }
}