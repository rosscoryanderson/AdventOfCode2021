namespace src.Features;

public class Polymiser
{
    // private string _polymer;
    private int _size;
    private Dictionary<string, long> _polymer;
    private Dictionary<string, string> _rules;
    private Dictionary<char, long> _characters;

    public Polymiser(List<string> input, int size = 2)
    {
        _size = size;
        _rules = new Dictionary<string, string>();
        _characters = new Dictionary<char, long>();
        _polymer = new Dictionary<string, long>();

        ParsePolymer(input[0]);
        
        input.RemoveRange(0, 2);
        ParseRules(input);
    }


    public long CalculateHighestLowestProduct(int steps = 10)
    {
        ExecuteRules(steps);
        var leastCommon = _characters.First(c => c.Value == _characters.Values.Min()).Value;
        var mostCommon = _characters.First(c => c.Value == _characters.Values.Max()).Value;

        return mostCommon - leastCommon;
    }

    private void ExecuteRules(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            _polymer = ExecuteRules();
        }
    }

    private Dictionary<string,long> ExecuteRules()
    {
        var polymer = new Dictionary<string, long>();

        foreach (var pattern in _polymer.Keys)
        {
            if (_rules.ContainsKey(pattern))
            {
                var firstPattern = $"{pattern[0]}{_rules[pattern]}";
                var secondPattern = $"{_rules[pattern]}{pattern[1]}";

                polymer = AddToPolymer(polymer, firstPattern, _polymer[pattern]);
                polymer = AddToPolymer(polymer, secondPattern, _polymer[pattern]);
                AddCharacter(_rules[pattern][0], _polymer[pattern]);
            }
            else
            {
                polymer = AddToPolymer(polymer, pattern, _polymer[pattern]);
            }
        }

        return polymer;
    } 

    private void ParsePolymer(string polymer)
    {
        var startIndex = 0;

        while (startIndex < polymer.Length)
        {
            if (startIndex < polymer.Length - 1)
            {
                var endIndex = startIndex + 2;
                var pattern = polymer[startIndex..endIndex];
                AddToPolymer(pattern);
            }

            var character = polymer[startIndex];
            AddCharacter(character);
            
            startIndex++;
        }
    }

    private void ParseRules(List<string> input)
    {
        foreach (var rule in input)
        {
            var ruleParts = rule.Split(" -> ");
            _rules.Add(ruleParts[0], ruleParts[1]);
        }
    }

    private void AddCharacter(char character, long value = 1)
    {
        if (!_characters.ContainsKey(character))
        {
            _characters.Add(character, 0);
        }

        _characters[character] += value;
    }

    private void AddToPolymer(string pattern)
    {
        if (!_polymer.ContainsKey(pattern))
        {
            _polymer.Add(pattern, 0);
        }

        _polymer[pattern]++;
    }
    
    private Dictionary<string, long> AddToPolymer(Dictionary<string, long> polymer, string pattern, long value = 0)
    {
        if (!polymer.ContainsKey(pattern))
        {
            polymer.Add(pattern, 0);
        }

        polymer[pattern] += value;

        return polymer;
    }
}