using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public class SolveResponse
{
    public int[] Sudoku { get; set; }
    public int BranchesUsed { get; set; }
    public bool Impossible { get; set; }

    public SolveResponse(int[] sudArray, int branches, bool impossible)
    {
        Sudoku = sudArray;
        BranchesUsed = branches;
        Impossible = impossible;
    }
}
