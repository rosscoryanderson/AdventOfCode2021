namespace src.Features;

public class Submarine
{
    
    private int _depth;
    private int _horizontalPosition;
    private int _aim;
    private readonly bool _useAim;

    private readonly List<string> _legalMoves = new()
    {
        "forward",
        "down",
        "up"
    };

    public Submarine(bool useAim = false)
    {
        _depth = 0;
        _horizontalPosition = 0;
        _aim = 0;
        _useAim = useAim;
    }

    public (int depth, int horizontalPosition) Move(string input)
    {
        var (direction, distance) = ParseInput(input);

        if (!string.IsNullOrEmpty(direction))
        {
            if (_useAim)
            {
                UpdatePositionWithAim(direction, distance);
            }
            else
            {
                UpdatePosition(direction, distance);
            }
        }

        return (_depth, _horizontalPosition);
    }

    public int CalculatePowerConsumption(List<string> input)
    {
        var diagnosticReportValues = ParseDiagnosticReport(input);

        var threshold = input.Count / 2;
        var valueSize = diagnosticReportValues.Keys.Count - 1;

        var gamma = 0;
        var epsilon = 0;

        foreach (var item in diagnosticReportValues)
        {
            if (item.Value > threshold)
            {
                gamma += (int)Math.Pow(2, (valueSize - item.Key));
            }
            else
            {
                epsilon += (int)Math.Pow(2, (valueSize - item.Key));
            }
        }

        return gamma * epsilon;
    }
    
    public int CalculateLifeSupportRating(List<string> input)
    {
        var oxygenGeneratorRatings = input;
        var co2ScrubberRatings = input;

        for (int i = 0; i < input[0].Length; i++)
        {
            var oxygenValues = ParseDiagnosticReport(oxygenGeneratorRatings);
            var oxygenThreshold = oxygenGeneratorRatings.Count / 2.0;
            var oxygenTarget = oxygenValues[i] >= oxygenThreshold ? '1' : '0';
            
            if (oxygenGeneratorRatings.Count > 1)
            {
                oxygenGeneratorRatings = oxygenGeneratorRatings.Where(x => x[i] == oxygenTarget).ToList();
            }
            
            var co2Values = ParseDiagnosticReport(co2ScrubberRatings);
            var co2Threshold = co2ScrubberRatings.Count / 2.0;
            var co2Target = co2Values[i] >= co2Threshold ? '0' : '1';
            
            if (co2ScrubberRatings.Count > 1)
            {
                co2ScrubberRatings = co2ScrubberRatings.Where(x => x[i] == co2Target).ToList();
            }

            if (oxygenGeneratorRatings.Count == 1 && co2ScrubberRatings.Count == 1) break;
        }

        var oxygenGeneratorRating = Convert.ToInt32(oxygenGeneratorRatings.First(), 2);
        var co2ScrubberRating = Convert.ToInt32(co2ScrubberRatings.First(), 2);

        return oxygenGeneratorRating * co2ScrubberRating;
    }

    

    private Dictionary<int, int> ParseDiagnosticReport(List<string> input)
    {
        var diagnosticReportValues = new Dictionary<int, int>();

        for (var i = 0; i < input[0].Length; i++)
        {
            diagnosticReportValues.Add(i, 0);
        }

        foreach (var value in input)
        {
            var bits = value.ToCharArray();

            for (int i = 0; i < bits.Length; i++)
            {
                var bitValue = bits[i] == '1' ? 1 : 0;

                diagnosticReportValues[i] += bitValue;
            }
        }

        return diagnosticReportValues;
    }

    private (string direction, int distance) ParseInput(string input)
    {
        var pieces = input.Split(' ');
        
        if (pieces.Length >= 2 && _legalMoves.Contains(pieces[0].ToLower()) && int.TryParse(pieces[1], out var distance))
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

    private void UpdatePositionWithAim(string direction, int distance)
    {
        switch (direction)
        {
            case "forward":
                _horizontalPosition += distance;
                _depth += distance * _aim;
                break;
            case "up":
                _aim -= distance;
                break;
            case "down":
                _aim += distance;
                break;
        }
    }
}