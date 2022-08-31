using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public class Solved
{
    public readonly SudokuTable CompleteTable;
    public readonly SudokuTable InProgressTable;

    readonly Queue<SudokuTable> _branches = new Queue<SudokuTable> ();
    public int BranchesUsedToSolve = 0;
    public bool Impossible { get; set; }
    public void AddOneCorrectValue(int x, int y)
    {
        if (Impossible)
        {
            return;
        }
        var correct = CompleteTable.GetSquare(x, y);
        var empty = InProgressTable.GetSquare(x, y);

        if (correct is not null && empty is not null && empty.Value == Values.NoValue)
            empty.InsertValue(correct.Value);
        else if (empty is not null && empty.Value != Values.NoValue)
            return;
    }
    SudokuTable SolvePuzzle()
    {
        if (InProgressTable.CorrectSquares.Count == 81)
        {
            return new SudokuTable(InProgressTable.GameSquares);
        } else if (InProgressTable.InCorrectSquares.Any(s => s.Value != Values.NoValue))
        {
            Impossible = true;
            return InProgressTable;
        }
        bool solved = false;
        SudokuTable tableToSolve = new SudokuTable(InProgressTable.GameSquares); 
        SudokuSquare squareToSolve;

        while (!solved)
        {
            if (tableToSolve.LeastPossibleValuesSquare == null)
            {
                if (!_branches.Any())
                {
                    Impossible = true;
                    return InProgressTable;

                }
                tableToSolve = _branches.Dequeue();  
            }
            else
            {
                squareToSolve = tableToSolve.LeastPossibleValuesSquare;
                int possibleCount = squareToSolve.PossibleCorrectValues.Count;

                if (possibleCount == 1)
                    squareToSolve.InsertValue(squareToSolve.PossibleCorrectValues.First());
                else if (possibleCount > 1)
                {
                    foreach (Values possibleValue in squareToSolve.PossibleCorrectValues)
                    {
                        squareToSolve.InsertValue(possibleValue);
                        CreateBranch(tableToSolve.GameSquares);
                    }
                }
                solved = tableToSolve.CorrectSquares.Count == 81;
            }
        }
        _branches.Clear();
        return tableToSolve;
    }
    void CreateBranch(List<SudokuSquare> squares)
    {
        _branches.Enqueue(new SudokuTable(squares));
        BranchesUsedToSolve++;
    }
    public Solved(SudokuTable startingpoint)
    {
        InProgressTable = new SudokuTable(startingpoint.GameSquares);
        CompleteTable = SolvePuzzle();
    }
}
