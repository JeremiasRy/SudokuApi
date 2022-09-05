using SudokuLibrary;

namespace SudokuTests;
[TestClass]
public class RandomSudokuTests
{
    [TestMethod]
    public async Task ReceiveARandomSudokuAccordingToDifficulty()
    {
        var randomSudoku1 = await RandomSudoku.GetRandomSudoku(0);
        var randomSudoku2 = await RandomSudoku.GetRandomSudoku(1);
        var randomSudoku3 = await RandomSudoku.GetRandomSudoku(2);
        Assert.AreNotEqual(randomSudoku1.Sudoku, randomSudoku2.Sudoku);
        Assert.AreNotEqual(randomSudoku2.Sudoku, randomSudoku3.Sudoku);
        Assert.AreNotEqual(randomSudoku3.Sudoku, randomSudoku1.Sudoku);

        var reMadeSudokuTable1 = await SudokuTable.BuildTable(randomSudoku1.Sudoku);
        var reMadeSudokuTable2 = await SudokuTable.BuildTable(randomSudoku2.Sudoku);
        var reMadeSudokuTable3 = await SudokuTable.BuildTable(randomSudoku3.Sudoku);
        Assert.AreEqual(35, SudokuTable.CorrectSquares(reMadeSudokuTable1.GameSquares).Count);
        Assert.AreEqual(25, SudokuTable.CorrectSquares(reMadeSudokuTable2.GameSquares).Count);
        Assert.AreEqual(15, SudokuTable.CorrectSquares(reMadeSudokuTable3.GameSquares).Count);

        try
        {
            var randomMalformDifficulty = await RandomSudoku.GetRandomSudoku(1232456783);
        } catch (Exception ex)
        {
            Assert.AreEqual("Difficulty was out of range", ex.Message);
        }

    }
}
