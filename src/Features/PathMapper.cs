using src.Domain;

namespace src.Features;

public class PathMapper
{
    private class RiskLevel
    {
        public int Energy { get; set; }
        public Coordinate Coordinate;
        public RiskLevel? ClosestNeighbour;

        public RiskLevel(int energy, Coordinate coordinate)
        {
            Energy = energy;
            Coordinate = coordinate;
        }
    }

    private Dictionary<Coordinate, int> _riskLevels;
    private int _tileWidth;
    private int _tileHeight;

    public PathMapper(List<string> input)
    {
        _riskLevels = new Dictionary<Coordinate, int>();
        _tileHeight = input.Count;
        _tileWidth = input[0].Length;

        ParseRiskReadings(input);
    }

    public int FindShortestPath()
    {
        var startCoord = new Coordinate(0, 0);
        var endCoord = _riskLevels.Keys
            .OrderByDescending(x => x.X)
            .ThenByDescending(y => y.Y)
            .First();
        
        var risk = Dijkstra(startCoord, endCoord);

        return risk;
    }

    public void ExpandGrid(int multiplier = 5)
    {
        var riskLevels = new Dictionary<Coordinate, int>(_riskLevels);

        foreach (var ((x, y), energy) in _riskLevels)
        {
            for (var i = 1; i < multiplier; i++)
            {
                var south = new Coordinate(x, y + _tileHeight * i);
                var modifiedEnergy = GetEnergy(energy, i);
                riskLevels.Add(south, modifiedEnergy);
            }
        }

        _riskLevels = riskLevels;
        
        riskLevels = new Dictionary<Coordinate, int>(_riskLevels);
        
        foreach (var ((x, y), energy) in _riskLevels)
        {
            for (var i = 1; i < multiplier; i++)
            {
                var east = new Coordinate(x + _tileWidth * i, y);
                riskLevels.Add(east, GetEnergy(energy, i));
            }
        }

        _tileHeight *= multiplier;
        _tileWidth *= multiplier;

        _riskLevels = riskLevels;
    }

    private int GetEnergy(int initialEnergy, int modifier)
    {
        var energy = initialEnergy + modifier;

        return energy > 9 ? energy - 9 : energy;
    }

    private int Dijkstra(Coordinate start, Coordinate end)
    {
        var (priorityQueue, nodeLookup) = InitialisePriorityQueue();
        var completed = new HashSet<RiskLevel>();

        var endNode = priorityQueue.First(rl => rl.Coordinate == end);
        
        var currentNode = priorityQueue.First(rl => rl.Coordinate == start);
        currentNode.Energy = 0;

        while (true)
        {
            var connections = GetConnections(currentNode.Coordinate, nodeLookup);
            
            foreach (var connection in connections)
            {
                if (completed.Contains(connection)) continue;
                
                var energy = _riskLevels[connection.Coordinate] + currentNode.Energy;

                if (energy < connection.Energy)
                {
                    connection.Energy = energy;
                    connection.ClosestNeighbour = currentNode;
                }
            }

            priorityQueue.Remove(currentNode);
            completed.Add(currentNode);
            
            if (currentNode == endNode || !priorityQueue.Any()) break;

             priorityQueue = Reprioritise(priorityQueue);

            currentNode = priorityQueue[0];
        }

        return endNode.Energy;
    }

    private (List<RiskLevel> priorityQueue, Dictionary<Coordinate, RiskLevel> nodeLookup) InitialisePriorityQueue()
    {
        var priorityQueue = new List<RiskLevel>();
        var nodeLookup = new Dictionary<Coordinate, RiskLevel>();
        
        foreach (var coord in _riskLevels.Keys)
        {
            var priorityRiskLevel = new RiskLevel(Int32.MaxValue, coord);
            
            priorityQueue.Add(priorityRiskLevel);
            nodeLookup.Add(coord, priorityRiskLevel);
        }

        return (priorityQueue, nodeLookup);
    }

    private List<RiskLevel> Reprioritise(List<RiskLevel> priorityQueue)
    {
        return priorityQueue.OrderBy(pq => pq.Energy).ToList();
    }

    private List<RiskLevel> GetConnections(Coordinate coord, Dictionary<Coordinate, RiskLevel> nodeLookup)
    {
        var connections = new List<RiskLevel>();
        
        var x = coord.X;
        var y = coord.Y;

        var north = new Coordinate(x, y - 1);
        var east = new Coordinate(x + 1, y);
        var south = new Coordinate(x, y + 1);
        var west = new Coordinate(x - 1, y);

        if (nodeLookup.ContainsKey(north))
        {
            connections.Add(nodeLookup[north]);
        }
            
        if (nodeLookup.ContainsKey(east))
        {
            connections.Add(nodeLookup[east]);
        }

        if (nodeLookup.ContainsKey(south))
        {
            connections.Add(nodeLookup[south]);
        }
            
        if (nodeLookup.ContainsKey(west))
        {
            connections.Add(nodeLookup[west]);
        }

        return connections;
    }

    private void ParseRiskReadings(List<string> input)
    {
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var energy = int.Parse(input[y][x].ToString());
                var coords = new Coordinate(x, y);
                _riskLevels.Add(coords, energy);
            }
        }
    }
}