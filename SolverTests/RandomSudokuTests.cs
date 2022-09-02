using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLibrary;

namespace SudokuTests;
[TestClass]
public class RandomSudokuTests
{
    [TestMethod]
    public void ReceiveARandomSudokuAccordingToDifficulty()
    {
        var randomSudoku = RandomSudoku.GetRandomSudoku(0);
        var randomSudoku2 = RandomSudoku.GetRandomSudoku(0);
        Assert.AreNotEqual(randomSudoku.Sudoku, randomSudoku2.Sudoku);
        var newSudoku = new SudokuTable(randomSudoku.Sudoku);
        if (newSudoku.Solved is not null)
        {
            Assert.IsFalse(newSudoku.Solved.Impossible);
            Assert.IsTrue(newSudoku.CorrectSquares.Count == 35);
        }
        randomSudoku = RandomSudoku.GetRandomSudoku(1);
        newSudoku = new SudokuTable(randomSudoku.Sudoku);
        if (newSudoku.Solved is not null)
        {
            Assert.IsFalse(newSudoku.Solved.Impossible);
            Assert.IsTrue(newSudoku.CorrectSquares.Count == 25);
        }
        randomSudoku = RandomSudoku.GetRandomSudoku(2);
        newSudoku = new SudokuTable(randomSudoku.Sudoku);
        if (newSudoku.Solved is not null)
        {
            Assert.IsFalse(newSudoku.Solved.Impossible);
            Assert.IsTrue(newSudoku.CorrectSquares.Count == 15);
        }
        try
        {
            randomSudoku = RandomSudoku.GetRandomSudoku(3);
        } catch (Exception ex)
        {
            Assert.IsTrue(ex is ArgumentException);
            Assert.AreEqual(ex.Message, "difficulty is out of range");
        }
    }
}
