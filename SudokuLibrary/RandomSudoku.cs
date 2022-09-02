using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public static class RandomSudoku
{
    public static SolveResponse GetRandomSudoku(int difficulty)
    {
        if (difficulty > 2) 
        {
            throw new ArgumentException(message: "difficulty was out of range");
        }
        int[] empty = new int[81];
        Random rand = new ();
        for (int i = 0; i < 81; i++)
        {
            empty[i] = 0;
        }
        SudokuTable sudokuTable = new(empty);

        for (int i = 0; i < 3; i++)
        {
            var square = sudokuTable.EmptySquares[rand.Next(0, sudokuTable.EmptySquares.Count - 1)];
            var posvalue = square.PossibleCorrectValues[rand.Next(0, square.PossibleCorrectValues.Count - 1)];
            square.InsertValue(square.PossibleCorrectValues[rand.Next(0, square.PossibleCorrectValues.Count)]);

            var check = new SudokuTable(sudokuTable.GameSquares.Select(x => (int)x.Value).ToArray());
            if (check.Solved is not null && check.Solved.Impossible)
            {
                sudokuTable = new SudokuTable(empty);
                i = 0;
            }
        }
        if (sudokuTable.Solved is not null)
            sudokuTable = new SudokuTable(sudokuTable.Solved.CompleteTable.GameSquares);

        switch (difficulty)
        {
            case 0:
                {
                    while (sudokuTable.CorrectSquares.Count > 35)
                    {
                        sudokuTable.CorrectSquares[rand.Next(0, sudokuTable.CorrectSquares.Count - 1)].InsertValue(Values.NoValue);
                    }
                }
                break;
            case 1:
                {
                    while (sudokuTable.CorrectSquares.Count > 25)
                    {
                        sudokuTable.CorrectSquares[rand.Next(0, sudokuTable.CorrectSquares.Count - 1)].InsertValue(Values.NoValue);
                    }
                }
                break;
            case 2:
                {
                    while (sudokuTable.CorrectSquares.Count > 15)
                    {
                        sudokuTable.CorrectSquares[rand.Next(0, sudokuTable.CorrectSquares.Count - 1)].InsertValue(Values.NoValue);
                    }
                }
                break;
        }
        return new SolveResponse(sudokuTable.GameSquares.Select(x => (int)x.Value).ToArray(), 0, false);
    }
}
