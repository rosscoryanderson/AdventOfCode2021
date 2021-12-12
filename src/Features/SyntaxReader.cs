namespace src.Features;

public class SyntaxReader
{
    private class Chunk
    {
        private Dictionary<char, char> _openingChars;
        private char _type;
        private Chunk? _parent;

        public Chunk()
        {
            _type = Char.MinValue;
            _parent = null;
            PopulateOpeningChars();
        }

        public Chunk(char openingSymbol, Chunk? parent = null)
        {
            _type = openingSymbol;
            _parent = parent;
            PopulateOpeningChars();
        }

        public Chunk? ReadSymbol(char symbol)
        {
            if (_openingChars.ContainsKey(symbol))
            {
                return new Chunk(symbol, this);
            }

            if (_openingChars[_type] == symbol)
            {
                return _parent;
            }

            return null;
        }

        public (Chunk? chunk, char symbol) MoveUp()
        {
            return (_parent, _openingChars[_type]);
        }

        private void PopulateOpeningChars()
        {
            _openingChars = new Dictionary<char, char>
            {
                {'(', ')'},
                {'[', ']'},
                {'{', '}'},
                {'<', '>'},
                {char.MinValue, char.MinValue}
            };
        }
    }

    private Dictionary<char, int> _errorScores;
    private Dictionary<char, int> _autocompleteScores;
    private List<string> _input;

    public SyntaxReader(List<string> input)
    {
        _errorScores = new Dictionary<char, int>
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137},
            {char.MinValue, 0}
        };
        _input = input;
        _autocompleteScores = new Dictionary<char, int>
        {
            {')', 1},
            {']', 2},
            {'}', 3},
            {'>', 4},
            {char.MinValue, 0}
        };
    }

    public int GetCorruptedPoints()
    {
        var totalPoints = 0;
        
        foreach (var line in _input)
        {
            totalPoints += GetCorruptedPoints(line);
        }

        return totalPoints;
    }

    public long AutoComplete()
    {
        var autocompleteScores = new List<long>();
        
        foreach (var line in _input)
        {
            var score = Autocomplete(line);
            if (score is not null)
            {
                autocompleteScores.Add(score.Value);
            }
        }

        var midpointIndex = (autocompleteScores.Count / 2);

        var x = autocompleteScores.OrderBy(x => x).ToList();
        
        return x[midpointIndex];
    }

    private long? Autocomplete(string line)
    {
        var index = 0;
        var root = new Chunk();
        var currentNode = root.ReadSymbol(line[index++]);

        if (currentNode is null)
        {
            return null;
        }

        while (index < line.Length)
        {
            currentNode = currentNode.ReadSymbol(line[index]);
            
            if (currentNode is null)
            {
                return null;
            }

            index++;

            if (index == line.Length && currentNode != root)
            {
                long totalScore = 0;

                while (currentNode != null)
                {
                    (currentNode, var closingCharacter) = currentNode.MoveUp();
                    if (currentNode != null)
                    {
                        totalScore = totalScore * 5 + _autocompleteScores[closingCharacter];
                    }
                }

                return totalScore;
            } 
        }

        return null;
    }

    private int GetCorruptedPoints(string line)
    {
        var index = 0;
        var parent = new Chunk(line[index++]);
        var currentNode = parent.ReadSymbol(line[index]);

        if (currentNode is null)
        {
            return GetScore(line[index]);
        }

        index++;

        while (index < line.Length)
        {
            currentNode = currentNode.ReadSymbol(line[index]);
            
            if (currentNode is null)
            {
                return GetScore(line[index]);
            }

            index++;
        }

        return 0;
    }

    private int GetScore(char symbol)
    {
        return _errorScores.ContainsKey(symbol) ? _errorScores[symbol] : 0;
    }
    
    
}