namespace src.Util;

public class MeasurementWindow
{
    private readonly MeasurementWindow? _previous;
    private readonly int _numReadings;
    private List<int> _readings;

    public MeasurementWindow(MeasurementWindow? previous, int current, int numReadings = 3)
    {
        _previous = previous;
        _numReadings = numReadings;
        _readings = new List<int> { current };
    }

    public bool TryAddReading(int reading)
    {
        if (_readings.Count > _numReadings)
            return false;
            
        _readings.Add(reading);
        
        return _readings.Count != _numReadings;
    }

    public bool IsIncrease()
    {
        if (_readings.Count != _numReadings || _previous == null)
            return false;

        return _readings.Sum() > _previous.Sum();
    }

    private int Sum()
    {
        return _readings.Sum();
    }
}