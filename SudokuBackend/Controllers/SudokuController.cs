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
    static int[]? ParseCoordinates(string coord)
    {
        try
        {
            var coordArray = coord.Split("-").Select(Int32.Parse).ToArray();
            if (coordArray.Length != 2 && coordArray[0] < 0 || coordArray[0] > 8 && coordArray[1] < 0 && coordArray[1] > 8)
            {
                return null;
            } else
            {
                return coordArray;
            }

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
    [HttpGet("/solveSquare/{coordinates}")]
    public IResult GetOneCorrectSquare(string coordinates, string sudoku)
    {
        _logger.LogInformation("Http Get request to solve one square // {0}", DateTime.UtcNow.ToString());

        var squareCoordinates = ParseCoordinates(coordinates);
        var sudArray = ParseSudoku(sudoku);
        if (squareCoordinates is not null && sudArray is not null)
        {
            SudokuTable sudokuTable = new(sudArray);
            return Results.Ok(sudokuTable.GetOneCorrectValue(squareCoordinates[0], squareCoordinates[1]));
        } else
        {
            string message = $"Sudoku should be 9 x 9, total 81 squares size, separate squares with commas";
            ModelState.AddModelError("Sudoku malform", message);
            return Results.BadRequest(ModelState);
        }
    }
}
