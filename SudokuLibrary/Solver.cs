using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public class Solver
{
    public readonly SudokuTable StartingPoint;
    public readonly SudokuTable SolvedTable;
    public SudokuTable? MiddlePoint;

    List<SudokuTable> _branches = new List<SudokuTable> (); 
    public int BranchesUsedToSolve { get; set; }
    public bool Impossible { get; set; }
    public SudokuTable SolvePuzzle()
    {
        return new SudokuTable(new int[82]);
    }
    public SudokuTable GiveOneCorrectValue(int x, int y) 
    {
        MiddlePoint = new SudokuTable(StartingPoint.GameSquares.Select(square => (int)square.Value).ToArray());
        var correct = SolvedTable.GetSquare(x, y);
        var empty = MiddlePoint.GetSquare(x, y);
        if (correct is not null && empty is not null)
            empty.InsertValue(correct.Value);
        return MiddlePoint;
    }

    bool DeadEnd(List<SudokuSquare> sudokuSquares)
    {
        return true;
    }

    void CreateBranch()
    {

    }
    public Solver(SudokuTable startingpoint)
    {
        StartingPoint = startingpoint;
        SolvedTable = SolvePuzzle();
    }
}
