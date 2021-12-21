using src.Domain;

namespace src.Features;

public class ProbeLauncher
{

    private Coordinate _min;
    private Coordinate _max;
    private int _xMin;
    private int _xMax;

    public ProbeLauncher(string input)
    {
        var (minimum, maximum) = ParseInput(input);

        _min = minimum;
        _max = maximum;
        
        _xMin = GetDistance(1, _min.X);
        _xMax = GetDistance(_xMin, _max.X);
    }

    public long LaunchTrickShot()
    {
        var yMax = 0L;

        for (int x = _xMin; x <= _xMax; x++)
        {
            for (int y = 100; y < 200; y++)
            {
                var coords = new Coordinate(x, y);
                var yValue = CalculateTrajectory(coords);

                if (yValue > yMax)
                {
                    yMax = yValue;
                }
            }
        }
        return yMax;
    }

    public long GetAllLandings()
    {
        var collisions = new List<Coordinate>();
        
        for (var x = 0; x <= 200; x++)
        {
            for (var y = -200; y < 200; y++)
            {
                var coords = new Coordinate(x, y);
                var collision = CalculateCollisions(coords);

                if (collision is not null)
                {
                    collisions.Add(collision);
                }
            }
        }

        return collisions.Count;
    }

    private int GetDistance(int startingX, int target)
    {
        var x = startingX;
        var xValue = 1;

        while (xValue < target)
        {
            x++;
            xValue = TotalDistance(x);
        }

        return x;
    }

    private int TotalDistance(int x)
    {
        return x * (x + 1) / 2;
    }

    private long CalculateTrajectory(Coordinate velocity)
    {
        var yMax = 0L;

        var currentVelocity = velocity;
        var currentPosition = new Coordinate(0, 0);
        var continueFlag = true;

        while (continueFlag)
        {
            (currentVelocity, currentPosition) = UpdateTrajectory(currentVelocity, currentPosition);

            var targetPosition = CheckPosition(currentPosition);

            continueFlag = targetPosition < 0;

            if (currentPosition.Y > yMax)
            {
                yMax = currentPosition.Y;
            }
            
            if (targetPosition == 0) return yMax;
            
            if (targetPosition > 0) return 0;
        }

        return 0;

    }
    
    private Coordinate? CalculateCollisions(Coordinate velocity)
    {
        var currentVelocity = velocity;
        var currentPosition = new Coordinate(0, 0);
        var continueFlag = true;

        while (continueFlag)
        {
            (currentVelocity, currentPosition) = UpdateTrajectory(currentVelocity, currentPosition);

            var targetPosition = CheckPosition(currentPosition);

            continueFlag = targetPosition < 0;
            
            if (targetPosition == 0) return velocity;
            
            if (targetPosition > 0) return null;
        }

        return null;

    }
    
    private (Coordinate velocity, Coordinate position) UpdateTrajectory(Coordinate startingVelocity, Coordinate startingPosition)
    {
        var xPosition = startingPosition.X + startingVelocity.X;
        var yPosition = startingPosition.Y + startingVelocity.Y;

        var xVelocity = startingVelocity.X > 0 ? startingVelocity.X - 1 : 0;
        var yVelocity = startingVelocity.Y - 1;
        
        var velocity = new Coordinate(xVelocity, yVelocity);
        var position = new Coordinate(xPosition, yPosition);

        return (velocity, position);
    }

    private int CheckPosition(Coordinate position)
    {
        var (x, y) = position;
        if (x >= _min.X && x <= _max.X && y >= _min.Y && y <= _max.Y)
        {
            // Collision
            return 0;
        }

        if (x < _max.X && y > _min.Y)
        {
            // Moving towards target
            return -1;
        }

        // Passed target;
        return 1;
    }

    private (Coordinate minimum, Coordinate maximum) ParseInput(string input)
    {
        const string partOneStringToRemove = "target area: x=";
        const string partTwoStringToRemove = "y=";
        
        var parts = input.Split(", ");

        var (xMin, xMax) = GetValues(parts[0], partOneStringToRemove.Length);
        var (yMin, yMax) = GetValues(parts[1], partTwoStringToRemove.Length);

        return (new Coordinate(xMin, yMin), new Coordinate(xMax, yMax));
    }

    private (int minimum, int maximum) GetValues(string parts, int length)
    {
        var values = parts
            .Remove(0, length)
            .Split("..").Select(int.Parse)
            .OrderBy(x => x)
            .ToList();

        return (values[0], values[1]);
    } 
}