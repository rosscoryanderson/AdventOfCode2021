namespace src.Features;

public class LanternFishCalculator
{
    private List<DailyFish> _dailyFish;

    private class DailyFish
    {
        public readonly double NumberOfFish;
        private int _timer;

        public DailyFish(double numberOfFish, int timer = 8)
        {
            NumberOfFish = numberOfFish;
            _timer = timer;
        }

        public void Update()
        {
            _timer = _timer == 0 ? 6 : _timer - 1;
        }

        public bool IsSpawnDay()
        {
            return _timer == 0;
        }
    }

    public LanternFishCalculator(List<string> input)
    {
        _dailyFish = new List<DailyFish>();

        foreach (var value in input)
        {
            var fishGroups = value
                .Split(',')
                .Select(int.Parse)
                .GroupBy(x => x)
                .ToList();
            
            foreach (var fish in fishGroups)
            {
                var numFish = fish.Count();
                var timer = fish.Key;
                _dailyFish.Add(new DailyFish(numFish, timer));
            }
        }
    }

    public double CalculateGrowthRate(int days)
    {
        for (var i = 0; i < days; i++)
        {
            var fishToAdd = _dailyFish
                .Where(dailyFish => dailyFish.IsSpawnDay())
                .Sum(dailyFish => dailyFish.NumberOfFish);
            
            _dailyFish.ForEach(dailyFish => dailyFish.Update());
            
            _dailyFish.Add(new DailyFish(fishToAdd));
        }
        
        return _dailyFish.Sum(dailyFish => dailyFish.NumberOfFish);
    }
}