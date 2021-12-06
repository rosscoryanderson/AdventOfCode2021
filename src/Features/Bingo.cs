namespace src.Features;

public class Bingo
{
    private class BingoValue 
    {
        public int Value { get; }
        public bool IsMarked {get; set;}

        public BingoValue(int value, bool isMarked)
        {
            Value = value;
            IsMarked = isMarked;
        }
    };

    private List<int> _drawnNumbers;
    private List<List<List<BingoValue>>> _boards;
    private Dictionary<int, List<BingoValue>> _boardNumbers;
    private int _boardSize;
    

    public Bingo(List<string> input, int boardSize = 5)
    {
        _drawnNumbers = new List<int>();
        _boards = new  List<List<List<BingoValue>>>();
        _boardNumbers = new Dictionary<int, List<BingoValue>>();
        _boardSize = boardSize;
        
        ParseDrawnNumbers(input);
        ParseBoards(input);
    }

    public int GetFinalScoreForGame()
    {
        var finalScore = 0;
        
        foreach (var number in _drawnNumbers)
        {
            if (!_boardNumbers.ContainsKey(number)) continue;

            _boardNumbers[number].ForEach(value => value.IsMarked = true);

            var unmarkedTotal = GetUnmarkedTotalOfCompletedBoard();

            if (unmarkedTotal > 0)
            {
                finalScore = number * unmarkedTotal;
                break;
            }
        }

        return finalScore;
    }
    
    public int GetFinalScoreForLosingGame()
    {
        var finalScore = 0;
        
        foreach (var number in _drawnNumbers)
        {
            if (!_boardNumbers.ContainsKey(number)) continue;

            _boardNumbers[number].ForEach(value => value.IsMarked = true);

            var unmarkedTotal = RemoveOrScoreBoard();

            if (unmarkedTotal > 0)
            {
                finalScore = number * unmarkedTotal;
                break;
            }
        }

        return finalScore;
    }

    private int RemoveOrScoreBoard()
    {
        var boardsToRemove = new  List<List<List<BingoValue>>>();
        
        foreach (var board in _boards)
        {
            if (!board.Any(row => row.All(value => value.IsMarked))) continue;
            
            if (_boards.Count == 1)
            {
                return UnmarkedScore(board);
            }

            boardsToRemove.Add(board);
        }

        foreach (var board in boardsToRemove)
        {
            _boards.Remove(board);
        }

        return 0;
    }

    private int GetUnmarkedTotalOfCompletedBoard()
    {
        foreach (var board in _boards)
        {
            if (board.Any(row => row.All(value => value.IsMarked)))
            {
                return UnmarkedScore(board);
            }
        }

        return 0;
    }

    private int UnmarkedScore(List<List<BingoValue>> board)
    {
        return board.Take(_boardSize) // The values are entered twice, need to take the first lot.
            .SelectMany(value => value)
            .Where(value => !value.IsMarked)
            .Sum(value => value.Value);
    }

    private void ParseBoards(List<string> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            if (string.IsNullOrEmpty(input[i])) continue;

            var board = new List<List<BingoValue>>();

            for (var j = 0; j < _boardSize; j++)
            {
                board.Add(new List<BingoValue>());
            }

            for (var j = 0; j < _boardSize; j++)
            {
                var row = new List<BingoValue>();

                var rowValues = input[i + j].Split(' ')
                    .Where(value => !string.IsNullOrEmpty(value))
                    .ToList();

                for (var k = 0; k < _boardSize; k++)
                {
                    var value = int.Parse(rowValues[k]);
                    var bingoValue = new BingoValue(value, false);
                    
                    row.Add(bingoValue);
                    board[k].Add(bingoValue); // add column

                    if (!_boardNumbers.ContainsKey(value))
                    {
                        _boardNumbers.Add(value, new List<BingoValue>());
                    }
                    _boardNumbers[value].Add(bingoValue);
                }
                board.Add(row);
            }
            _boards.Add(board);
            i += _boardSize;
        }
    }

    private void ParseDrawnNumbers(List<string> input)
    {
        foreach (var number in input[0].Split(','))
        {
            _drawnNumbers.Add(int.Parse(number));
        }
        input.RemoveAt(0);
    }
}