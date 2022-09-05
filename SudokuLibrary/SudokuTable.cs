using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;
public class SudokuTable
{
    readonly List<SudokuSquare> CompleteTable;
    public readonly List<SudokuSquare> GameSquares;
    public bool Impossible { get; set; }
    public static List<SudokuSquare> EmptySquares(List<SudokuSquare> squares) => squares.Where(square => square.Value == Values.NoValue).ToList();
    public static List<SudokuSquare> CorrectSquares(List<SudokuSquare> squares) => squares.Where(square => square.IsCorrect).ToList();
    public static List<SudokuSquare> InCorrectSquares(List<SudokuSquare> squares) => squares.Where(square => !square.IsCorrect).ToList();
    public static SudokuSquare? LeastPossibleValuesSquare(List<SudokuSquare> squares) { 
            if (!EmptySquares(squares).Any())
                return null;
            else if (EmptySquares(squares).Where(square => square.PossibleCorrectValues.Count == 1).Any(square => square.PossibleCorrectValues.First() == Values.NoValue))
                return null; //Dead end
            else
                return EmptySquares(squares).OrderBy(square => square.PossibleCorrectValues.Count).First();
        } 
    public SolveResponse GetCompletedSudoku()
    {
        if (Impossible)
            throw new Exception("Sudoku is impossible to solve");
        return new SolveResponse(CompleteTable.Select(x => (int)x.Value).ToArray());
    }
    public SolveResponse GetOneCorrectValue(int x, int y)
    {
        var squareToInsert = GetSquare(x, y, false);
        var squareCorrect = GetSquare(x, y, true);
        squareToInsert.InsertValue(squareCorrect.Value);
        if (Impossible)
            throw new Exception("Sudoku is impossible to solve");
        else
            return new SolveResponse(GameSquares.Select(x => (int)x.Value).ToArray());
    }
    public SudokuSquare GetSquare(int x, int y, bool FromCompleted)
    {
        if (x > 8 || y > 8)
            throw new ArgumentException(message: $"Coordinates out of range x: {x} y: {y}");

        var square = FromCompleted ? CompleteTable.Find(square => square.X == x && square.Y == y) : GameSquares.Find(square => square.X == x && square.Y == y);
        if (square is not null)
            return square;
        else
            throw new Exception(message: "Didn't find square");
    }

    public static async Task<SudokuTable> BuildTable(int[] sudoku)
    {
        if (sudoku.Length != 81)
        {
            throw new ArgumentException(message: $"Sudoku was malform, should have length of 81 was {sudoku.Length}");
        }
        if (sudoku.Any(x => x > 9))
        {
            string malformValues = String.Join(",", sudoku.Where(x => x > 9));
            throw new ArgumentException(message: $"Sudoku had incorrect value for a square. Check values: {malformValues}");
        }
        var startingSquares = new List<SudokuSquare>();
        int count = 0;
        int box;
        for (int iy = 0; iy < 9; iy++)
        {
            if (iy < 3)
                box = 0;
            else if (iy > 2 && iy < 6)
                box = 3;
            else
                box = 6;
            for (int ix = 0; ix < 9; ix++)
            {
                if (ix == 3 || ix == 6)
                    box++;
                startingSquares.Add(new SudokuSquare((Values)sudoku[count], ix, iy, box, startingSquares));
                count++;
            }
        }
        var solvedSquares = await Task.Run(() => SolvePuzzle(startingSquares));

        return new SudokuTable(startingSquares, solvedSquares, CorrectSquares(solvedSquares).Count < 81);
    }

    static List<SudokuSquare> SolvePuzzle(List<SudokuSquare> sudoku)
    {
        Queue<List<SudokuSquare>> _branches = new();
        var start = new List<SudokuSquare>();

        foreach (var sudokuSquare in sudoku)
            start.Add(new SudokuSquare(sudokuSquare.Value, sudokuSquare.X, sudokuSquare.Y, sudokuSquare.Box, start));

        if (CorrectSquares(start).Count == 81) // Solved already
        {
            return start;
        }
        else if (InCorrectSquares(start).Any(s => s.Value != Values.NoValue)) //Impossible to solve
        {
            return start;
        }
        bool solved = false;
        List<SudokuSquare> tableToSolve = start;
        SudokuSquare squareToSolve;

        while (!solved)
        {
            var leastPos = LeastPossibleValuesSquare(tableToSolve);
            if (leastPos == null)
            {
                if (!_branches.Any())
                {
                    return tableToSolve; //Impossible
                }
                tableToSolve = _branches.Dequeue();
            }
            else
            {
                squareToSolve = leastPos;
                int possibleCount = squareToSolve.PossibleCorrectValues.Count;

                if (possibleCount == 1)
                    squareToSolve.InsertValue(squareToSolve.PossibleCorrectValues.First());
                else if (possibleCount > 1)
                {
                    foreach (Values possibleValue in squareToSolve.PossibleCorrectValues)
                    {
                        squareToSolve.InsertValue(possibleValue);
                        CreateBranch(tableToSolve);
                    }
                }
                solved = CorrectSquares(tableToSolve).Count == 81;
            }
        }
        _branches.Clear();
        return tableToSolve;

        void CreateBranch(List<SudokuSquare> squares)
        {
            List<SudokuSquare> newBranch = new();

            foreach (var square in squares)
                newBranch.Add(new SudokuSquare(square.Value, square.X, square.Y, square.Box, newBranch));

            _branches.Enqueue(newBranch);
        }
    }

    private SudokuTable(List<SudokuSquare> starting, List<SudokuSquare> solvedTable, bool impossible)
    {
        GameSquares = starting;
        CompleteTable = solvedTable;
        Impossible = impossible;
    }
}
