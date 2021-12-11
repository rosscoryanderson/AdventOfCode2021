namespace src.Features;

public class DigitDisplay
{
    private List<(string[] signalPattern, string[] outputValue)> _inputReadings;
    private Dictionary<string, int> _digitPatterns;
    // private Dictionary<char, char> _finalisedWirings;
    // private Dictionary<char, List<char>> _possibleWirings;

    public DigitDisplay(List<string> input)
    {
        _inputReadings = new List<(string[] signalPattern, string[] outputValue)>();
        
        foreach (var reading in input)
        {
            var separatedValues = reading.Split(" | ");
            var signalPattern = separatedValues[0].Split(' ');
            var outputValue = separatedValues[1].Split(' ');
            _inputReadings.Add((signalPattern, outputValue));
        }

        _digitPatterns = new Dictionary<string, int>
        {
            {"abcefg", 0 },
            {"cf", 1 },
            {"acdeg", 2 },
            {"acdfg", 3  },
            {"bcdf", 4 },
            {"abdfg", 5 },
            {"abdefg", 6 },
            {"acf", 7 },
            {"abcdefg", 8 },
            {"abcdfg", 9 },
        };
        
    }

    public int GetEasyDigits()
    {
        var easyDigits = 0;

        foreach (var reading in _inputReadings)
        {
            easyDigits += GetNumberOfEasyDigits(reading.outputValue);
        }

        return easyDigits;
    }

    public int GetTotalOutputDigits()
    {
        var totalOutput = 0;

        foreach (var reading in _inputReadings)
        {
            var mapping = GetMapping(reading.signalPattern);

            var currentOutputValue = 0d;

            for (int i = 0; i < reading.outputValue.Length; i++)
            {
                var mappedCharacters = reading.outputValue[i].Select(x => mapping[x]).OrderBy(x => x).ToArray();
                var index = new string(mappedCharacters);

                var value = _digitPatterns[index];;
                var exponential = reading.outputValue.Length - i - 1;
                currentOutputValue += value * Math.Pow(10, exponential);
            }

            totalOutput += (int)currentOutputValue;
        }

        return totalOutput;
    }

    private Dictionary<char, char> GetMapping(string[] signalPattern)
    {
        var mapping = new Dictionary<char, char>();
        var oneDigits = GetOneValue(signalPattern);
        var (c, f) = GetRightHandValues(oneDigits, signalPattern);
        var a = signalPattern.First(x => x.Length == 3).ToCharArray().First(x => x != c && x != f);

        var dg = signalPattern
            .First(x => x.Contains(a) && x.Contains(c) && x.Contains(f) && x.Length == 5)
            .Where(x => x != c && x != f && x != a)
            .ToArray();

        var (b, d, g) = GetBdgValues(dg, signalPattern, c, f);
            
        mapping.Add(a, 'a');
        mapping.Add(b, 'b');
        mapping.Add(c, 'c');
        mapping.Add(d, 'd');
        mapping.Add(f, 'f');
        mapping.Add(g, 'g');

        var e = signalPattern.First(x => x.Length == 7).First(x => !mapping.ContainsKey(x));

        mapping.Add(e, 'e');

        return mapping;
    }

    private (char a, char b) GetRightHandValues(char[] oneDigitValues, string[] signalPattern)
    {
        var sixDigitWithFirstChar = signalPattern.FirstOrDefault(x =>
            x.Length == 6 && x.Contains(oneDigitValues[1]) && !x.Contains(oneDigitValues[0]));

        return string.IsNullOrEmpty(sixDigitWithFirstChar) 
            ? (oneDigitValues[1], oneDigitValues[0]) 
            : (oneDigitValues[0], oneDigitValues[1]);
    }

    private (char b, char d, char g) GetBdgValues(char[] dgChars, string[] signalPattern, char c, char f)
    {
        var fourDigitChars = signalPattern.First(x => x.Length == 4).ToCharArray();

        var (d, g) = fourDigitChars.Contains(dgChars[0]) 
            ? (dgChars[0], dgChars[1]) 
            : (dgChars[1], dgChars[0]);

        var ineligibleChars = new[] {c, f, d, g};

        var b = fourDigitChars.First(x => !ineligibleChars.Contains(x));

        return (b, d, g);
    }

    private char[] GetOneValue(string[] values)
    {
        return values.First(x => x.Length == 2).ToCharArray();
    }

    private int GetNumberOfEasyDigits(string[] outputValue)
    {
        var values = 0;
        foreach (var value in outputValue)
        {
            if (IsEasyValue(value))
            {
                values++;
            }
        }

        return values;
    }

    private bool IsEasyValue(string value)
    {
        return value.Length is 2 or 3 or 4 or 7;
    }
}