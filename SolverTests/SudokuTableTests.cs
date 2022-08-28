namespace SudokuTests;
using SudokuLibrary;
using System.Linq;

[TestClass]
public class SudokuTableTests
{
    static readonly int[] sudEmpty = new int[81];
    static readonly int[] sudCorrect = new int[81]
    {
            1,2,3,4,5,6,7,8,9,
            4,5,6,7,8,9,1,2,3,
            7,8,9,1,2,3,4,5,6,
            2,3,4,5,6,7,8,9,1,
            5,6,7,8,9,1,2,3,4,
            8,9,1,2,3,4,5,6,7,
            3,4,5,6,7,8,9,1,2,
            6,7,8,9,1,2,3,4,5,
            9,1,2,3,4,5,6,7,8,
    };
    static readonly int[] sudCorrectNineEmpty = new int[81]
    {
            1,2,3,4,5,6,0,0,0,
            4,5,6,7,8,9,1,2,3,
            0,0,0,1,2,3,4,5,6,
            2,3,4,5,6,7,8,9,1,
            5,6,7,0,0,0,2,3,4,
            8,9,1,2,3,4,5,6,7,
            3,4,5,6,7,8,9,1,2,
            6,7,8,9,1,2,3,4,5,
            9,1,2,3,4,5,6,7,8,
    };
    SudokuTable sudTable = new(sudEmpty);

    [TestInitialize] 
    public void InitializeTable()
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudEmpty[count] = 0;
                count++;
            }
        }

        Assert.AreEqual(81, sudEmpty.Length);
        sudTable = new SudokuTable(sudEmpty);
    }
    [TestMethod]
    public void TestTableXYBox()
    {
        int count = 0;
        int box = 0;
        SudokuSquare? square; 

        for (int y = 0; y < 9; y++)
        {
            if (y < 3)
                box = 0;
            else if (y > 2 && y < 6)
                box = 3;
            else
                box = 6;
            for (int x = 0; x < 9; x++)
            {
                if (x == 3 || x == 6)
                    box++;
                square = sudTable.GetSquare(x, y);
                if (square is not null)
                {
                    Assert.AreEqual((Values)sudEmpty[count], square.Value);
                    Assert.AreEqual(x, square.X);
                    Assert.AreEqual(y, square.Y);
                    Assert.AreEqual(box, square.Box);
                }
                count++;
            }
        }
        Assert.IsNull(sudTable.GetSquare(9999, 9999));
    }

    [TestMethod]
    public void TestSudokuSquareIsCorrectAndPossibleValues()
    {
        var square = sudTable.GetSquare(0, 0);
        if (square is not null)
        {
            Assert.IsFalse(sudTable.CorrectSquares.Any());
            Assert.AreEqual(9, square.PossibleCorrectValues().Count);

            square.InsertValue(Values.One);
            Assert.AreEqual(1, sudTable.CorrectSquares.Count());
        }
        square = sudTable.GetSquare(1, 0);
        if (square is not null)
        {
            Assert.AreEqual(8, square.PossibleCorrectValues().Count);
        }

        sudTable = new SudokuTable(sudCorrect);

        Assert.AreEqual(81, sudTable.CorrectSquares.Count);
        Assert.IsFalse(sudTable.EmptySquares.Any());

        Assert.AreEqual(0, sudTable.EmptySquares.Count);

        sudTable = new SudokuTable(sudCorrectNineEmpty);
        square = sudTable.GetSquare(0, 0);
        if (square is not null)
            Assert.IsTrue(square.IsCorrect);
        square = sudTable.GetSquare(6, 0);
        if (square is not null)
            Assert.IsFalse(square.IsCorrect);

        Assert.AreEqual(72, sudTable.CorrectSquares.Count);
        Assert.AreEqual(9, sudTable.EmptySquares.Count);

        square = sudTable.GetSquare(6, 0);
        if (square is not null)
        {
            square.InsertValue(Values.Nine);
            Assert.AreEqual(10, sudTable.InCorrectSquares.Count);
            square.InsertValue(Values.Eight);
            Assert.AreEqual(10, sudTable.InCorrectSquares.Count);
            square.InsertValue(Values.Seven);
            Assert.AreEqual(8, sudTable.InCorrectSquares.Count);
        }
        square = sudTable.GetSquare(7, 0);
        if (square is not null)
        {
            Assert.AreEqual((Values)8, square.PossibleCorrectValues().First());
            square.InsertValue(Values.Eight);
        }
        square = sudTable.GetSquare(8, 0);
        if (square is not null)
            Assert.AreEqual(Values.Nine, square.PossibleCorrectValues().First());
    }
}
