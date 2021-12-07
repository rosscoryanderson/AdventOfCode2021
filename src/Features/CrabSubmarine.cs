namespace src.Features;

public class CrabSubmarine
{
    private List<int> _crabs;

    public CrabSubmarine(List<string> input)
    {
        _crabs = new List<int>();

        foreach (var inputLine in input)
        {
            var crabs = inputLine.Split(',').Select(int.Parse).ToList();
            _crabs.AddRange(crabs);
        }
    }

    public int CalculateFuelUsage()
    {
        var min = _crabs.Min();
        var max = _crabs.Max();

        var minFuelUseage = int.MaxValue;

        for (var target = min; target < max; target++)
        {
            var fuelUsed = _crabs.Select(crab => Math.Abs(target - crab)).Sum();

            minFuelUseage = fuelUsed < minFuelUseage ? fuelUsed : minFuelUseage;
        }

        return minFuelUseage;
    }

    public int CalculateIncreasingFuelUsage()
    {
        var min = _crabs.Min();
        var max = _crabs.Max();

        var minFuelUseage = int.MaxValue;

        for (var target = min; target < max; target++)
        {
            var fuelUsed = _crabs.Select(crab => Enumerable.Range(1, Math.Abs(target - crab))
                    .Aggregate(0, (current, item) => current + item))
                .Sum();

            minFuelUseage = fuelUsed < minFuelUseage ? fuelUsed : minFuelUseage;
        }

        return minFuelUseage;
    }

}