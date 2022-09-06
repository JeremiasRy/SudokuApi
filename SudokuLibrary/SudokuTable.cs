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
    public SolveResponse GetOneCorrectValue(int row, int column)
    {
        var squareToInsert = GetSquare(row, column, false);
        var squareCorrect = GetSquare(row, column, true);
        squareToInsert.InsertValue(squareCorrect.Value);
        if (Impossible)
            throw new Exception("Sudoku is impossible to solve");
        else
            return new SolveResponse(GameSquares.Select(square => (int)square.Value).ToArray());
    }
    public SudokuSquare GetSquare(int row, int column, bool FromCompleted)
    {
        if (row > 8 || column > 8)
            throw new ArgumentException(message: $"Coordinates out of range row: {row} column: {column}");

        var square = FromCompleted ? CompleteTable.Find(square => square.Row == row && square.Column == column) : GameSquares.Find(square => square.Row == row && square.Column == column);
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
        for (int iRow = 0; iRow < 9; iRow++)
        {
            if (iRow < 3)
                box = 0;
            else if (iRow > 2 && iRow < 6)
                box = 3;
            else
                box = 6;
            for (int iColumn = 0; iColumn < 9; iColumn++)
            {
                if (iColumn == 3 || iColumn == 6)
                    box++;
                startingSquares.Add(new SudokuSquare((Values)sudoku[count], iRow, iColumn, box, startingSquares));
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
            start.Add(new SudokuSquare(sudokuSquare.Value, sudokuSquare.Row, sudokuSquare.Column, sudokuSquare.Box, start));

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
                newBranch.Add(new SudokuSquare(square.Value, square.Row, square.Column, square.Box, newBranch));

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
