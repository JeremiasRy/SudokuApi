using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;

namespace SudokuBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class SudokuController : ControllerBase
{
    private readonly ILogger _logger;
    public SudokuController(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger("Sudoku controls");
    }
    static int[]? ParseSudoku(string sud)
    {
        try
        {
            var sudArray = sud.Split(",").Select(Int32.Parse).ToArray();
            if (sudArray.Length == 81)
                return sudArray;
            else
                return null;
        } catch (Exception)
        {
            return null;
        }

    }
    static int[]? ParseCoordinates(string x, string y)
    {
        try
        {
            int xCoordinate = int.Parse(x);
            int yCoordinate = int.Parse(y);
            if (xCoordinate < 0 || xCoordinate > 8 || yCoordinate < 0 || yCoordinate > 8)
                return null;
            else
                return new [] { xCoordinate, yCoordinate };
        } catch (Exception)
        {
            return null;
        }

    }
    [HttpGet("/solve/{sudoku}")]
    public IResult GetCompletedSudoku(string sudoku)
    {
        _logger.LogInformation("Http Get request to solve // {0}", DateTime.UtcNow.ToString());

        var sudArray = ParseSudoku(sudoku);
        if (sudArray is not null)
        {
            SudokuTable sudokuTable = new(sudArray);
            return Results.Ok(sudokuTable.GetCompletedSudoku());
        } else
        {
            string message = $"Sudoku should be 9 x 9, total 81 squares size, separate squares with commas";
            ModelState.AddModelError("Sudoku malform", message);
            return Results.BadRequest(ModelState);
        }

    }
    [HttpGet("/solveSquare/{x}/{y}/{sudoku}")]
    public IResult GetOneCorrectSquare(string x,string y, string sudoku)
    {
        _logger.LogInformation("Http Get request to solve one square // {0}", DateTime.UtcNow.ToString());

        var squareCoordinates = ParseCoordinates(x,y);
        var sudArray = ParseSudoku(sudoku);

        if (squareCoordinates is not null && sudArray is not null)
        {
            SudokuTable sudokuTable = new(sudArray);
            return Results.Ok(sudokuTable.GetOneCorrectValue(squareCoordinates[0], squareCoordinates[1]));
        } else
        {
            string message = $"Sudoku should be 9 x 9, total 81 squares size, separate squares with commas. Coordinates should be from 0 to 8";
            ModelState.AddModelError("Sudoku malform", message);
            return Results.BadRequest(ModelState);
        }
    }
}
