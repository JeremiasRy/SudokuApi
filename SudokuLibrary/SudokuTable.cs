﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public class SudokuTable
{
    public List<SudokuSquare> GameSquares;
    public SudokuSquare GetSquare(int x, int y) => GameSquares.Where(square => square.X == x && square.Y == y).First();
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
                GameSquares.Add(new SudokuSquare(sudoku[count], ix, iy, box));
                count++;
            }
        }
    }
    public SudokuTable(List<SudokuSquare> squares)
    {
        GameSquares = new List<SudokuSquare>(squares); 
    }
}
