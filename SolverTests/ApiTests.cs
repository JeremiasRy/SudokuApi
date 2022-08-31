using SudokuBackend.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
