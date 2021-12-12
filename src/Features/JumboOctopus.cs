using src.Domain;

namespace src.Features;

public class JumboOctopus
{
    private class Octopus
    {
        public readonly Coordinate Coordinate;
        private int _energy;

        public Octopus(Coordinate coordinate, int energy)
        {
            Coordinate = coordinate;
            _energy = energy;
        }

        public bool IsFlashing => _energy > 9;

        public void Reset() => _energy = 0;
        public void Increase() => _energy++;

        public bool TryIncrease()
        {
            if (_energy > 9) return false;
            
            _energy++;
            return true;

        }
    }

    private Dictionary<Coordinate, Octopus> _grid;
    private List<Octopus> _octopusList;
    private int _ySize;
    private int _xSize;

    public JumboOctopus(List<string> input)
    {
        _grid = new Dictionary<Coordinate, Octopus>();
        _octopusList = new List<Octopus>();
        _ySize = input.Count;
        _xSize = input[0].Length;
        PopulateGrid(input);
    }

    public int SimulateSteps(int steps = 100)
    {
        var numFlashes = 0;

        for (int i = 0; i < steps; i++)
        {
            numFlashes += PerformStep();
        }

        return numFlashes;
    }

    public int Synchronise()
    {
        var step = 1;
        var targetFlashes = _xSize * _ySize;

        while (true)
        {
            var numFlash = PerformStep();
            if (numFlash == targetFlashes)
            {
                return step;
            }

            step++;
        }
    }

    private int PerformStep()
    {
        _octopusList.ForEach(o => o.Increase());

        _octopusList
            .Where(o => o.IsFlashing)
            .ToList()
            .ForEach(EmanateFlash);

        var flashedThisStep = _octopusList.Where(o => o.IsFlashing).ToList();

        var numFlashes = flashedThisStep.Count;
            
        flashedThisStep.ForEach(o => o.Reset());

        return numFlashes;
    }

    private void EmanateFlash(Octopus octopus)
    {
        var x = octopus.Coordinate.X;
        var y = octopus.Coordinate.Y;

        if (HasNorthNeighbours(y))
        {
            var north = new Coordinate(x, y - 1);
            Emanate(north);

            if (HasEastNeighbours(x))
            {
                var northEast = new Coordinate(x + 1, y - 1);
                Emanate(northEast);
            }
            
            if (HasWestNeighbours(x))
            {
                var northWest = new Coordinate(x - 1, y - 1);
                Emanate(northWest);
            }
        }
        
        if (HasSouthNeighbours(y))
        {
            var south = new Coordinate(x, y + 1);
            Emanate(south);

            if (HasEastNeighbours(x))
            {
                var southEast = new Coordinate(x + 1, y + 1);
                Emanate(southEast);
            }
            
            if (HasWestNeighbours(x))
            {
                var southWest = new Coordinate(x - 1, y + 1);
                Emanate(southWest);
            }
        }
        
        if (HasEastNeighbours(x))
        {
            var east = new Coordinate(x + 1, y);
            Emanate(east);
        }
            
        if (HasWestNeighbours(x))
        {
            var west = new Coordinate(x - 1, y);
            Emanate(west);
        }
    }

    private void Emanate(Coordinate coordinate)
    {
        var octopus = _grid[coordinate];

        if (octopus.TryIncrease() && octopus.IsFlashing)
        {
            EmanateFlash(octopus);
        }
    }
    
    private void PopulateGrid(List<string> input)
    {
        for (var y = 0; y < _ySize; y++)
        {
            for (var x = 0; x < _xSize; x++)
            {
                var coordinate = new Coordinate(x, y);
                var energy = int.Parse(input[y][x].ToString());
                var octopus = new Octopus(coordinate, energy);
                _grid.Add(coordinate, octopus);
                _octopusList.Add(octopus);
            }
        }
    }

    private bool HasNorthNeighbours(int y) => y > 0;
    private bool HasEastNeighbours(int x) => x < _xSize - 1;
    private bool HasSouthNeighbours(int y) => y < _ySize - 1;
    private bool HasWestNeighbours(int x) => x > 0;
}