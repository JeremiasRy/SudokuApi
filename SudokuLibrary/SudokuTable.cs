using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;
public class SudokuTable
{
    public readonly List<SudokuSquare> GameSquares;
    public readonly Solved? Solved;
    public SudokuSquare GetSquare(int x, int y) 
    {
        if (x > 8 || y > 8)
        {
            throw new ArgumentException(message: "Coordinates out of range");
        }
        var square = GameSquares.Find(square => square.X == x && square.Y == y);
        if (square is not null)
        {
            return square;
        } else
        {
            throw new Exception(message: "Didn't find square");
        }  
    }
    public List<SudokuSquare> EmptySquares => GameSquares.Where(square => square.Value == Values.NoValue).ToList();
    public List<SudokuSquare> CorrectSquares => GameSquares.Where(square => square.IsCorrect).ToList();
    public List<SudokuSquare> InCorrectSquares => GameSquares.Where(square => !square.IsCorrect).ToList();
    public SudokuSquare? LeastPossibleValuesSquare { get
        {
            if (!EmptySquares.Any())
            {
                return null;
            }
            if (EmptySquares.Where(square => square.PossibleCorrectValues.Count == 1).Any(square => square.PossibleCorrectValues.First() == Values.NoValue))
            {
                return null;
            } else
            {
                return EmptySquares.OrderBy(square => square.PossibleCorrectValues.Count).First();
            }
        } }
    public SolveResponse GetCompletedSudoku()
    {
        if (Solved is not null && !Solved.Impossible)
            return new SolveResponse(Solved.CompleteTable.GameSquares.Select(square => (int)square.Value).ToArray(), Solved.BranchesUsedToSolve, false);   
        else
            return new SolveResponse(Array.Empty<int>(), 0, true);
    

    }
    public SolveResponse GetOneCorrectValue(int x, int y)
    {
        if (Solved is not null && !Solved.Impossible)
        {
            Solved.AddOneCorrectValue(x, y);
            return new SolveResponse(Solved.StartingPoint.GameSquares.Select(square => (int)square.Value).ToArray(), Solved.BranchesUsedToSolve, false);
        }
        else
            return new SolveResponse(Array.Empty<int>(), 0, true);

    }
    public SudokuTable(int[] sudoku)
    {
        if (sudoku.Length != 81)
        {
            throw new ArgumentException(message: $"sudoku was malform, should have length of 81 was {sudoku.Length}");
        }
        GameSquares = new List<SudokuSquare>();
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
                GameSquares.Add(new SudokuSquare((Values)sudoku[count], ix, iy, box, this.GameSquares));
                count++;
            }
        }
        Solved = new Solved(this);
    }
    public SudokuTable(List<SudokuSquare> sudokuSquares) //Create a copy of sudoku table
    {
        GameSquares = new List<SudokuSquare>();
        foreach (SudokuSquare square in sudokuSquares)
        {
            GameSquares.Add(new SudokuSquare(square.Value, square.X, square.Y, square.Box, this.GameSquares));
        }
    }
}
