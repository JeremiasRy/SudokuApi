namespace SudokuTests;
using SudokuLibrary;
using System.Linq;
using UnitTests;

[TestClass]
public class SudokuTableTests
{
    [TestMethod]
    public async Task TestTableXYBoxAndGetSquare()
    {
        SudokuTable sudokuTable = await SudokuTable.BuildTable(SudokuArrays.Empty());
        int count = 0;
        int box;
        SudokuSquare square;
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
                square = sudokuTable.GetSquare(ix, iy, false);
                Assert.AreEqual(square.Row, ix);
                Assert.AreEqual(square.Column, iy);
                Assert.AreEqual(square.Box, box);
                Assert.AreEqual((Values)SudokuArrays.Empty()[count], square.Value);
                count++;
            }
        }
        sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudCorrect);
        count = 0;
        for (int iRow = 0; iRow < 9; iRow++)
        {
            if (iRow < 3)
                box = 0;
            else if (iRow > 2 && iRow < 6)
                box = 3;
            else
                box = 6;
            for (int iColumn = 0; iColumn < 9; iColumn++)
            {
                if (iColumn == 3 || iColumn == 6)
                    box++;
                square = sudokuTable.GetSquare(iRow, iColumn, false);
                Assert.AreEqual(square.Row, iColumn);
                Assert.AreEqual(square.Column, iRow);
                Assert.AreEqual(square.Box, box);
                Assert.AreEqual((Values)SudokuArrays.sudCorrect[count], square.Value);
                count++;
            }
        }
        try
        {
            sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudMalform);
        } catch (Exception ex)
        {
            Assert.IsTrue(ex is ArgumentException);
            Assert.AreEqual(ex.Message, "Sudoku had incorrect value for a square. Check values: 98,1789");
        }
        try
        {
            sudokuTable.GetSquare(9999, 9999, false);
        } catch (Exception ex)
        {
            Assert.IsTrue(ex is ArgumentException);
            Assert.AreEqual(ex.Message, "Coordinates out of range x: 9999 y: 9999");
        }
    }
    [TestMethod]
    public async Task TestStaticListMethods()
    {
        SudokuTable sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudCorrectNineEmpty);
        Assert.IsTrue(9 == SudokuTable.EmptySquares(sudokuTable.GameSquares).Count);
        Assert.IsTrue(72 == SudokuTable.CorrectSquares(sudokuTable.GameSquares).Count);
        Assert.IsTrue(9 == SudokuTable.InCorrectSquares(sudokuTable.GameSquares).Count);

        sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudHardAlmostEmpty);
        Assert.IsTrue(5 == SudokuTable.CorrectSquares(sudokuTable.GameSquares).Count);
        Assert.IsTrue(76 == SudokuTable.EmptySquares(sudokuTable.GameSquares).Count);
        Assert.IsTrue(76 == SudokuTable.InCorrectSquares(sudokuTable.GameSquares).Count);

        sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudImpossible);
        Assert.IsTrue(5 == SudokuTable.CorrectSquares(sudokuTable.GameSquares).Count);
        Assert.IsTrue(74 == SudokuTable.EmptySquares(sudokuTable.GameSquares).Count);
        Assert.IsTrue(76 == SudokuTable.InCorrectSquares(sudokuTable.GameSquares).Count);
    }
    [TestMethod]
    public async Task TestCompletedTable()
    {
        SudokuTable sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudHard);
        Assert.IsFalse(sudokuTable.Impossible);
        Assert.AreNotEqual(sudokuTable.GameSquares.Select(square => (int)square.Value).ToArray(), sudokuTable.GetCompletedSudoku().Sudoku);
        sudokuTable = await SudokuTable.BuildTable(sudokuTable.GetCompletedSudoku().Sudoku);
        Assert.IsFalse(sudokuTable.Impossible);

        sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudHardAlmostEmpty);
        SudokuTable compTable = await SudokuTable.BuildTable(sudokuTable.GetCompletedSudoku().Sudoku);
        Assert.AreNotEqual(SudokuTable.CorrectSquares(sudokuTable.GameSquares), SudokuTable.CorrectSquares(compTable.GameSquares));

        sudokuTable = await SudokuTable.BuildTable(SudokuArrays.Empty());
        Assert.IsFalse(sudokuTable.Impossible);
        Assert.IsTrue(SudokuTable.EmptySquares(sudokuTable.GameSquares).Count == 81);
        sudokuTable = await SudokuTable.BuildTable(sudokuTable.GetCompletedSudoku().Sudoku);
        Assert.IsTrue(SudokuTable.CorrectSquares(sudokuTable.GameSquares).Count == 81);

        sudokuTable = await SudokuTable.BuildTable(SudokuArrays.sudImpossible);
        Assert.IsTrue(sudokuTable.Impossible);
        try
        {
            sudokuTable.GetCompletedSudoku();
        } catch (Exception ex)
        {
            Assert.AreEqual("Sudoku is impossible to solve", ex.Message);
        }
    }
}
