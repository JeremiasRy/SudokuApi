using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLibrary;

namespace SudokuLibrary;

public class SolveResponse
{
    public int[] Sudoku { get; set; }
    public SolveResponse(int[] sudArray)
    {
        Sudoku = sudArray;
    }
}
