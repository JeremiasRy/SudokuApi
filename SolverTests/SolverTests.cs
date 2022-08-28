namespace SudokuTests;

using SudokuLibrary;
using System.Linq;

[TestClass]
public class SolverTests
{
    SudokuTable sudokuTable = new(new int[81]);

    [TestInitialize] 
    public void Initialize()
    {
        int[] sud = new int[81];
        for (int i = 0; i < sud.Length; i++)
        {
            sud[i] = 0;
        }
        sudokuTable = new SudokuTable(sud);

    }
    [TestMethod]
    public void SolverTableIsEqualToSudokuTable()
    {
        Assert.AreEqual(sudokuTable, sudokuTable.Solver.StartingPoint);
    }
    [TestMethod]
    public void SolverSolvesPuzzle()
    {

    }
    [TestMethod]
    public void SolverInformsImpossibleToSolve()
    {

    }
    [TestMethod]
    public void SolverSolvesEmptyPuzzle()
    {

    }
}
