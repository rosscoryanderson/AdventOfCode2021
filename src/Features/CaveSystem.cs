namespace src.Features;

public class CaveSystem
{
    private class Cave
    {
        private readonly string _name;
        public readonly HashSet<Cave> Connections;
        public bool IsSmallCave { get; }

        public Cave(string name)
        {
            _name = name;
            Connections = new HashSet<Cave>();
            IsSmallCave = CheckSmallCave(name);
        }

        public Cave? TryVisit => _name == "start" ? null : this;
        

        private static bool CheckSmallCave(string name)
        {
            return !IsStartOrEnd(name) && !char.IsUpper(name[0]);
        }

        private static bool IsStartOrEnd(string name)
        {
            return name is "start" or "end";
        }
    }

    private Dictionary<string, Cave> _caves;
    private int _cavesVisited;

    public CaveSystem(List<string> input)
    {
        _caves = new Dictionary<string, Cave>();
        _cavesVisited = 0;

        ParseConnections(input);
    }

    public int GetNumberOfPaths(bool canVisitSmallCavesTwice = false)
    {
        var start = _caves["start"];
        _cavesVisited = 0;

        foreach (var cave in start.Connections)
        {
            var smallCaves = new List<Cave>();
            if (cave.IsSmallCave)
            {
                smallCaves.Add(cave);
            }
            TraverseCaves(cave, smallCaves, canVisitSmallCavesTwice);
        }

        return _cavesVisited;
    }

    private void TraverseCaves(Cave parentCave, List<Cave> smallCaves, bool canVisitSmallCavesTwice = false)
    {
        foreach (var connectedCave in parentCave.Connections)
        {
            var cave = connectedCave.TryVisit;

            if (cave == null)
            {
                continue;
            }

            if (cave == _caves["end"])
            {
                _cavesVisited++;
                continue;
            }

            var visitedSmallCaves = new List<Cave>(smallCaves);
            
            if (cave.IsSmallCave)
            {
                if (visitedSmallCaves.Contains(cave))
                {
                    if (!canVisitSmallCavesTwice) continue;

                    if (visitedSmallCaves.Distinct().Count() == visitedSmallCaves.Count)
                    {
                        visitedSmallCaves.Add(cave);
                    }
                    else continue;
                }
                else
                {
                    visitedSmallCaves.Add(cave);
                }
            }

            TraverseCaves(cave, visitedSmallCaves, canVisitSmallCavesTwice);
        }
    }

    private void ParseConnections(List<string> input)
    {
        foreach (var line in input)
        {
            var caves = line.Split('-');

            var firstCave = GetOrAddCave(caves[0]);
            var secondCave = GetOrAddCave(caves[1]);

            firstCave.Connections.Add(secondCave);
            secondCave.Connections.Add(firstCave);
        }
    }

    private Cave GetOrAddCave(string name)
    {
        if (_caves.ContainsKey(name))
        {
            return _caves[name];
        }

        var cave = new Cave(name);
        _caves.Add(name, cave);

        return cave;
    }
}