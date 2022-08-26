namespace SolverTests;
using SudokuLibrary;
using System.Linq;

[TestClass]
public class SudokuTableTests
{
    SudokuTable sudTable;
    readonly int[] sud = new int[81];
    readonly int[] sudCorrect = new int[81]
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
    readonly int[] sudCorrectNineEmpty = new int[81]
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

    [TestInitialize] 
    public void InitializeTable()
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                sud[count] = j;
                count++;
            }
        }
        Assert.AreEqual(1, sud[0]);
        Assert.AreEqual(9, sud[17]);
        Assert.AreEqual(7, sud[24]);

        Assert.AreEqual(81, sud.Length);
        sudTable = new SudokuTable(sud);
    }
    [TestMethod]
    public void TestTableConstructor()
    {
        int count = 0;
        Assert.AreEqual(81, sudTable.GameSquares.Count);

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Assert.AreEqual(sud[count], sudTable.GameSquares.Where(square => square.X == x && square.Y == y).First().Value);
                count++;
            }
        }
        Assert.AreEqual(2, sudTable.GameSquares.Where(x => x.X == 6 && x.Y == 0).First().Box);
        Assert.AreEqual(5, sudTable.GameSquares.Where(x => x.X == 8 && x.Y == 3).First().Box);
        Assert.AreEqual(7, sudTable.GameSquares.Where(x => x.X == 4 && x.Y == 6).First().Box);

        sudTable = new SudokuTable(sudCorrect);
        count = 0;
        Assert.AreEqual(81, sudTable.GameSquares.Count);

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Assert.AreEqual(sudCorrect[count], sudTable.GameSquares.Where(square => square.X == x && square.Y == y).First().Value);
                count++;
            }
        }
        Assert.AreEqual(3, sudTable.GameSquares.Where(x => x.X == 0 && x.Y == 3).First().Box);
        Assert.AreEqual(5, sudTable.GameSquares.Where(x => x.X == 6 && x.Y == 3).First().Box);
        Assert.AreEqual(8, sudTable.GameSquares.Where(x => x.X == 8 && x.Y == 8).First().Box);
    }
    [TestMethod]
    public void TestSudokuSquare()
    {
        sudTable = new SudokuTable(sudCorrectNineEmpty);
        Assert.IsTrue(sudTable.GetSquare(0,0).HasValue);
        Assert.IsFalse(sudTable.GetSquare(6,0).HasValue);

        Assert.AreEqual(72, sudTable.GameSquares.Where(x => x.HasValue).Count());
        Assert.AreEqual(9, sudTable.GameSquares.Where(x => !x.HasValue).Count());
    }
}
