namespace src.Features;

public class PacketOperatorCalculator
{
    private List<long> _values;
    private Func<long, long> CalculateFunction; 

    public PacketOperatorCalculator(int typeId)
    {
        _values = new List<long>();

        CalculateFunction = typeId switch
        {
            0 => CalculateSum,
            1 => CalculateProduct,
            2 => CalculateMinimum,
            3 => CalculateMaximum,
            // case 4:
            //     CalculateFunction = CalculateGreaterThan;
            //     break;
            5 => CalculateGreaterThan,
            6 => CalculateLessThan,
            7 => CalculateEqualTo,
            _ => throw new NotImplementedException()
        };
    }

    public long Calculate(long value)
    {
        return CalculateFunction(value);
    }

    private long CalculateSum(long value)
    {
        _values.Add(value);
        return _values.Sum();
    }

    private long CalculateProduct(long value)
    {
        _values.Add(value);
        return _values.Aggregate(1L, (total, current) => current * total);
    }

    private long CalculateMinimum(long value)
    {
        _values.Add(value);
        return _values.Min();
    }

    private long CalculateMaximum(long value)
    {
        _values.Add(value);
        return _values.Max();
    }

    private long CalculateGreaterThan(long value)
    {
        if (_values.Any())
        {
            return _values[0] > value
                ? 1
                : 0;
        }
        _values.Add(value);
        return 0;
    }

    private long CalculateLessThan(long value)
    {
        if (_values.Any())
        {
            return _values[0] < value
                ? 1
                : 0;
        }
        _values.Add(value);
        return 0;
    }

    private long CalculateEqualTo(long value)
    {
        if (_values.Any())
        {
            return _values[0] == value
                ? 1
                : 0;
        }
        _values.Add(value);
        return 0;
    }
}