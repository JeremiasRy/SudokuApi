namespace SudokuTests;

using SudokuLibrary;
using UnitTests;
using System.Linq;

[TestClass]
public class SolverTests
{
    SudokuTable sudokuTable = new SudokuTable(new int[81]);

    [TestInitialize] 
    public void Initialize()
    {
        sudokuTable = new SudokuTable(SudokuArrays.Empty());
    }
    [TestMethod]
    public void SolverStartingPointIsNotEqualSudokuTable()
    {
        if (sudokuTable.Solved is not null)
           Assert.AreNotEqual(sudokuTable, sudokuTable.Solved.StartingPoint);
    }
    [TestMethod]
    public void SolverSolvedTableIsNotEqualToSdokuTableAndInfiniteLoopIsNotForming()
    {
        if (sudokuTable.Solved is not null)
        {
            Assert.AreNotEqual(sudokuTable, sudokuTable.Solved.CompleteTable);
            Assert.IsNull(sudokuTable.Solved.CompleteTable.Solved);
        }
    }
    [TestMethod]
    public void SolverSolvesPuzzle()
    {
        sudokuTable = new SudokuTable(SudokuArrays.sudCorrectNineEmpty);
        if (sudokuTable.Solved is not null)
        {
            Assert.AreNotEqual(sudokuTable.GameSquares, sudokuTable.Solved.CompleteTable);
            Assert.AreEqual(81, sudokuTable.Solved.CompleteTable.CorrectSquares.Count);
        }
        sudokuTable = new SudokuTable(SudokuArrays.sudHard);
        if (sudokuTable.Solved is not null)
        {
            Assert.AreNotEqual(sudokuTable.GameSquares, sudokuTable.Solved.CompleteTable);
            Assert.AreEqual(81, sudokuTable.Solved.CompleteTable.CorrectSquares.Count);
        }
        sudokuTable = new SudokuTable(SudokuArrays.sudHardAlmostEmpty);
        if (sudokuTable.Solved is not null)
        {
            Assert.AreNotEqual(sudokuTable.GameSquares, sudokuTable.Solved.CompleteTable);
            Assert.AreEqual(81, sudokuTable.Solved.CompleteTable.CorrectSquares.Count);
        }
    }
    [TestMethod]
    public void SolverInformsImpossibleToSolve()
    {
        sudokuTable = new SudokuTable(SudokuArrays.sudImpossible);
        if (sudokuTable.Solved is not null)
            Assert.IsTrue(sudokuTable.Solved.Impossible);


    }
    [TestMethod]
    public void SolverSolvesEmptyPuzzle()
    {
        if (sudokuTable.Solved is not null)
        {
            Assert.AreEqual(81, sudokuTable.Solved.CompleteTable.CorrectSquares.Count);
            Assert.AreEqual(0, sudokuTable.Solved.StartingPoint.CorrectSquares.Count);
            Assert.AreNotEqual(sudokuTable.Solved.StartingPoint, sudokuTable.Solved.CompleteTable);
        }
    }
    [TestMethod]
    public void SolverGivesOneCorrectValue()
    {
        if (sudokuTable.Solved is not null)
        {
            var correctSquare = sudokuTable.Solved.CompleteTable.GetSquare(3, 5);
            var solveResponse = sudokuTable.GetOneCorrectValue(3, 5);
            if (correctSquare is not null)
            {
                Assert.AreEqual(solveResponse.Sudoku[48], (int)correctSquare.Value);
            }
        }
        sudokuTable = new SudokuTable(SudokuArrays.sudHardAlmostEmpty);

        if (sudokuTable.Solved is not null)
        {
            var correctSquare = sudokuTable.Solved.CompleteTable.GetSquare(3, 5);
            var solveResponse = sudokuTable.GetOneCorrectValue(3, 5);
            if (correctSquare is not null)
            {
                Assert.AreEqual(solveResponse.Sudoku[48], (int)correctSquare.Value);
            }
        }
        sudokuTable = new SudokuTable(SudokuArrays.sudImpossible);

        if (sudokuTable.Solved is not null)
        {
            Assert.AreEqual(Array.Empty<int>(), sudokuTable.GetCompletedSudoku().Sudoku);
            Assert.AreEqual(Array.Empty<int>(), sudokuTable.GetOneCorrectValue(8,8).Sudoku);
        }

    }
}
