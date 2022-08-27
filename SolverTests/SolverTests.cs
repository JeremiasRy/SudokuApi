namespace SolverTests;
using SudokuLibrary;
using System.Linq;

[TestClass]
public class SudokuTableTests
{
    static readonly int[] sud = new int[81];
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
    SudokuTable sudTable = new(sud);

    [TestInitialize] 
    public void InitializeTable()
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sud[count] = 0;
                count++;
            }
        }

        Assert.AreEqual(81, sud.Length);
        sudTable = new SudokuTable(sud);
    }
    [TestMethod]
    public void TestTable()
    {
        int count = 0;
        Assert.AreEqual(81, sudTable.GameSquares.Count);

        sudTable = new SudokuTable(sudCorrect);
        count = 0;
        Assert.AreEqual(81, sudTable.GameSquares.Count);

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Assert.AreEqual((Values)sudCorrect[count], sudTable.GetSquare(x,y).Value);
                count++;
            }
        }
        Assert.IsNull(sudTable.GetSquare(9999, 9999));

        Assert.AreEqual(3, sudTable.GetSquare(0, 3).Box);
        Assert.AreEqual(5, sudTable.GetSquare(6,3).Box);
        Assert.AreEqual(8, sudTable.GetSquare(8,8).Box);

    }

    [TestMethod]
    public void TestSudokuSquare()
    {
        Assert.IsFalse(sudTable.GameSquares.Any(x => x.IsCorrect));
        Assert.AreEqual(9, sudTable.GetSquare(0, 0).PossibleCorrectValues().Count());

        sudTable.GetSquare(0, 0).InsertValue(Values.One);
        Assert.AreEqual(1, sudTable.GameSquares.Where(x => x.IsCorrect).Count());

        var table = sudTable.GameSquares;
        var listValues = sudTable.GetSquare(1, 0).PossibleCorrectValues();

        Assert.AreEqual(8, sudTable.GetSquare(1, 0).PossibleCorrectValues().Count());

        sudTable = new SudokuTable(sudCorrect);

        Assert.AreEqual(81, sudTable.GameSquares.Where(x => !x.Value.Equals(Values.NoValue)).Count());
        Assert.IsFalse(sudTable.GameSquares.Any(x => x.Value.Equals(Values.NoValue)));

        Assert.AreEqual(81, sudTable.GameSquares.Where(x => x.IsCorrect).Count());
        Assert.IsFalse(sudTable.GameSquares.Any(x => x.Value.Equals(Values.NoValue)));

        sudTable = new SudokuTable(sudCorrectNineEmpty);

        Assert.IsTrue(!sudTable.GetSquare(0,0).Value.Equals(Values.NoValue));
        Assert.IsFalse(!sudTable.GetSquare(6,0).Value.Equals(Values.NoValue));

        Assert.AreEqual(72, sudTable.GameSquares.Where(x => !x.Value.Equals(Values.NoValue)).Count());
        Assert.AreEqual(9, sudTable.GameSquares.Where(x => x.Value.Equals(Values.NoValue)).Count());

        Assert.AreEqual(72, sudTable.GameSquares.Where(x => x.IsCorrect).Count());
        Assert.AreEqual(9, sudTable.GameSquares.Where(x => !x.IsCorrect).Count());

        sudTable.GetSquare(6, 0).InsertValue(Values.Nine);
        Assert.AreEqual(10, sudTable.GameSquares.Where(x => !x.IsCorrect).Count());
        sudTable.GetSquare(6, 0).InsertValue(Values.Eight);
        Assert.AreEqual(10, sudTable.GameSquares.Where(x => !x.IsCorrect).Count());
        sudTable.GetSquare(6, 0).InsertValue(Values.Seven);
        Assert.AreEqual(8, sudTable.GameSquares.Where(x => !x.IsCorrect).Count());

        Assert.AreEqual((Values)8, sudTable.GetSquare(7, 0).PossibleCorrectValues().First());
        sudTable.GetSquare(7, 0).InsertValue(Values.Eight);
        Assert.AreEqual((Values)9, sudTable.GetSquare(8, 0).PossibleCorrectValues().First());

    }
}
