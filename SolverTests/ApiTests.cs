using SudokuBackend.Controllers;
using Microsoft.AspNetCore.Http;
using UnitTests;

namespace ApiTests;

[TestClass]
public class ApiGet
{
    [TestMethod] 
    public void ApiGetSolvedReturnsSolvedArray()
    {

    }
    [TestMethod]
    public void ApiInformsMalformattedSudoku()
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        try
        {
            controller.GetCompletedSudoku("0,0,0");
        } catch (Exception ex)
        {
            Assert.IsTrue(ex is ArgumentException);
            Assert.AreEqual(ex.Message, "sudoku was malform, should have length of 81 was 3");
        }

    }
    [TestMethod]
    public void ApiInformsImpossibleToSolve()
    {

    }
    [TestMethod]
    public void ApiReturnsOneCorrectValue()
    {

    }
}
