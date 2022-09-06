using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public static class RandomSudoku
{
    public static async Task<SolveResponse> GetRandomSudoku(int difficulty)
    {
        Random random = new Random();
        SudokuTable sudokuTable = await SudokuTable.BuildTable(new int[81]);
        bool impossible = true;

        while (impossible)
        {
            List<int> values = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] valuesToInsert = new int[5];

            for (int i = 0; i < valuesToInsert.Length; i++)
            {
                int index = random.Next(values.Count - 1);
                valuesToInsert[i] = values.ElementAt(index);
                values.RemoveAt(index);
            }
            int[] sudokuArray = new int[81];

            for (int i = 0; i < valuesToInsert.Length; i++)
            {
                sudokuArray[random.Next(80)] = valuesToInsert[i];
            }
            sudokuTable = await SudokuTable.BuildTable(sudokuArray);
            impossible = sudokuTable.Impossible;

            List<SudokuSquare> emptySquares;
            SudokuSquare squareToFill;

            switch (difficulty)
            {
                case 0:
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            emptySquares = SudokuTable.EmptySquares(sudokuTable.GameSquares);
                            squareToFill = emptySquares.ElementAt(random.Next(emptySquares.Count - 1));
                            squareToFill.InsertValue(sudokuTable.GetSquare(squareToFill.Row, squareToFill.Column, true).Value);
                        }
                    }
                    break;
                case 1:
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            emptySquares = SudokuTable.EmptySquares(sudokuTable.GameSquares);
                            squareToFill = emptySquares.ElementAt(random.Next(emptySquares.Count - 1));
                            squareToFill.InsertValue(sudokuTable.GetSquare(squareToFill.Row, squareToFill.Column, true).Value);
                        }
                    }
                    break;
                case 2:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            emptySquares = SudokuTable.EmptySquares(sudokuTable.GameSquares);
                            squareToFill = emptySquares.ElementAt(random.Next(emptySquares.Count - 1));
                            squareToFill.InsertValue(sudokuTable.GetSquare(squareToFill.Row, squareToFill.Column, true).Value);
                        }
                    }
                    break;
                default:
                    {
                        throw new ArgumentException("Difficulty was out of range");
                    }

            }
        }
        return new SolveResponse(sudokuTable.GameSquares.Select(x => (int)x.Value).ToArray());
    }   
}
