using SudokuBackend.Controllers;
using UnitTests;

namespace ApiTests;

[TestClass]
public class ApiPost
{
    [TestMethod] 
    public void ApiGetSolvedReturnsSolvedArray()
    {

    }
    [TestMethod]
    public void ApiInformsMalformattedSudoku()
    {
        
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        var result = controller.GetCompletedSudoku("0,0,00,0,0,0,");

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
