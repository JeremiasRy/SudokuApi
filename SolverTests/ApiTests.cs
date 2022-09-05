using SudokuBackend.Controllers;
using Microsoft.AspNetCore.Mvc;
using UnitTests;

namespace ApiTests;

[TestClass]
public class ApiGet
{
    [TestInitialize]
    public void InitializeTests()
    {
    }
    [TestMethod] 
    public async Task ApiGetSolvedReturnsSolvedArray()
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        IActionResult result = await controller.GetCompletedSudoku(String.Join(",",SudokuArrays.sudHard));
        Assert.IsTrue(result is OkObjectResult);
        var keepGoing = result as OkObjectResult;

       
    }
    [TestMethod]
    public async Task ApiInformsMalformattedSudoku()
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        IActionResult result = await controller.GetCompletedSudoku(String.Join(",", SudokuArrays.sudMalform));
        Assert.IsTrue(result is BadRequestObjectResult);
        var keepGoing = result as BadRequestObjectResult;        
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
