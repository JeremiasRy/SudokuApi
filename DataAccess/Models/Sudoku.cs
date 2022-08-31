using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

public class Sudoku
{
    public int[] SudokuArray { get; set; }
    public Sudoku(int[] sud)
    {
        SudokuArray = sud;
    }
}
