using SudokuBackend.Controllers;
using Microsoft.AspNetCore.Mvc;
using UnitTests;
using SudokuLibrary;
using System.Collections.Generic;

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
        if (result is not OkObjectResult keepGoing)
            return;

        Assert.IsTrue(keepGoing.Value is SolveResponse);
        if (keepGoing.Value is not SolveResponse response)
            return;

        Assert.IsTrue(response.Sudoku.Length == 81);
        var check = await SudokuTable.BuildTable(response.Sudoku);
        Assert.AreEqual(81, SudokuTable.CorrectSquares(check.GameSquares).Count); 
              
    }
    [TestMethod]
    public async Task ApiInformsMalformattedSudoku() //I don't know how to get the string value from seriazibleError so this test is pretty bad.
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        IActionResult result = await controller.GetCompletedSudoku(String.Join(",", SudokuArrays.sudMalform));
        Assert.IsTrue(result is BadRequestObjectResult);
    }
    [TestMethod]
    public async Task ApiInformsImpossibleToSolve() //Same as ApiInformsMalformattedSudoku
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        IActionResult result = await controller.GetCompletedSudoku(String.Join(",", SudokuArrays.sudImpossible));
        Assert.IsTrue(result is BadRequestObjectResult);

    }
    [TestMethod]
    public async Task ApiReturnsOneCorrectValue()
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        IActionResult result = await controller.GetOneCorrectSquare(0,0,String.Join(",", SudokuArrays.sudHard));
        Assert.IsTrue(result is OkObjectResult);
        if (result is not OkObjectResult keepGoing)
            return;

        Assert.IsTrue(keepGoing.Value is SolveResponse);
        if (keepGoing.Value is not SolveResponse response)
            return;

        var original = await SudokuTable.BuildTable(SudokuArrays.sudHard);
        var check = await SudokuTable.BuildTable(response.Sudoku);
        Assert.IsFalse(original.GetSquare(0, 0, false).IsCorrect);
        Assert.IsTrue(check.GetSquare(0, 0, false).IsCorrect);
    }
    [TestMethod]
    public async Task ApiReturnsInCorrectList()
    {
        var controller = new SudokuController(new Microsoft.Extensions.Logging.LoggerFactory());
        IActionResult result = await controller.GetIncorrectCoordinates(String.Join(",", SudokuArrays.sudHard));
        Assert.IsTrue(result is OkObjectResult);
        if (result is not OkObjectResult keepGoing)
            return;
        Assert.IsTrue(keepGoing.Value is List<SudokuSquare>);
        if (keepGoing.Value is not List<SudokuSquare> resultsList)
            return;
        Assert.IsTrue(resultsList.Count == SudokuArrays.sudHard.Where(x => x == 0).Count());
    }
}
