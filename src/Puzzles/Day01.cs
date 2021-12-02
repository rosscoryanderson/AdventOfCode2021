using src.Base;
using src.Util;

namespace src.Puzzles;

public class Day01 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var previous = -1;
        var increases = 0;

        foreach (var value in Input)
        {
            if (!int.TryParse(value, out var reading)) continue;
            
            if (previous < 0)
            {
                previous = reading;
                continue;
            }

            if (reading > previous)
            {
                increases++;
            }
                
            previous = reading;
        }

        return increases.ToString();
    }

    public string Puzzle2()
    {
        MeasurementWindow previous = null;
        var increases = 0;
        var activeMeasurementWindows = new List<MeasurementWindow>();

        foreach (var value in Input)
        {
            if (!int.TryParse(value, out var reading)) continue;

            var currentMeasurementWindow = new MeasurementWindow(previous, reading);
            
            var completedMeasurementWindows = new List<MeasurementWindow>();
            foreach (var window in activeMeasurementWindows)
            {
                if (window.TryAddReading(reading)) continue;
                
                if(window.IsIncrease())
                    increases++;

                completedMeasurementWindows.Add(window);
            }

            activeMeasurementWindows.RemoveAll(window => completedMeasurementWindows.Contains(window));
            activeMeasurementWindows.Add(currentMeasurementWindow);
            
            previous = currentMeasurementWindow;
        }
        
        return increases.ToString();
    }
}