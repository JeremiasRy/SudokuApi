using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;
public class SudokuTable
{
    public readonly List<SudokuSquare> GameSquares;
    public readonly Solver Solver;
    public SudokuSquare? GetSquare(int x, int y) => GameSquares.Find(square => square.X == x && square.Y == y);
    public List<SudokuSquare> EmptySquares => GameSquares.Where(square => square.Value.Equals(Values.NoValue)).ToList();
    public List<SudokuSquare> CorrectSquares => GameSquares.Where(square => square.IsCorrect).ToList();
    public List<SudokuSquare> InCorrectSquares => GameSquares.Where(square => !square.IsCorrect).ToList();  

    public SudokuTable(int[] sudoku)
    {
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
        Solver = new Solver(this);
    }
    public SudokuTable(List<SudokuSquare> sudokuSquares) //Create a copy of sudoku table
    {
        GameSquares = new List<SudokuSquare>();
        foreach (SudokuSquare square in sudokuSquares)
        {
            GameSquares.Add(new SudokuSquare(square.Value, square.X, square.Y, square.Box, this.GameSquares));
        }
        Solver = new Solver(this);
    }
}
