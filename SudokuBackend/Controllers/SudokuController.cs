using Microsoft.AspNetCore.Mvc;

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
    static int[] ParseSudoku(string sud)
    {
        try
        {
            return sud.Split(",").Select(Int32.Parse).ToArray();
        } catch (Exception)
        {
            return Array.Empty<int>();
        }

    }
    static int[] ParseCoordinates(string x, string y)
    {
        try
        {
            int xCoordinate = int.Parse(x);
            int yCoordinate = int.Parse(y);
            if (xCoordinate < 0 || xCoordinate > 8 || yCoordinate < 0 || yCoordinate > 8)
                return Array.Empty<int>();
            else
                return new [] { xCoordinate, yCoordinate };
        } catch (Exception)
        {
            return Array.Empty<int>();
        }

    }
    [HttpGet("/solve/{sudoku}")]
    public IResult GetCompletedSudoku(string sudoku)
    {
        int[] sudokuArray = ParseSudoku(sudoku);
        try
        {
            var sudokuTable = new SudokuTable(sudokuArray);
            return Results.Ok(sudokuTable.GetCompletedSudoku());
        } catch (Exception ex)
        {
            ModelState.AddModelError("Bad request", ex.Message);
            return Results.BadRequest(ModelState);
        }

    }
    [HttpGet("/solveSquare/{x}/{y}/{sudoku}")]
    public IResult GetOneCorrectSquare(string x,string y, string sudoku)
    {
        _logger.LogInformation("Http Get request to solve one square // {0}", DateTime.UtcNow.ToString());

        int[] squareCoordinates = ParseCoordinates(x,y);
        int[] sudArray = ParseSudoku(sudoku);

        try 
        {
            SudokuTable sudokuTable = new(sudArray);
            return Results.Ok(sudokuTable.GetOneCorrectValue(squareCoordinates[0], squareCoordinates[1]));
        } catch (Exception ex)
        {
            ModelState.AddModelError("Sudoku malform", ex.Message);
            return Results.BadRequest(ModelState);
        }
    }
    [HttpGet("/randomsudoku/{difficulty}")]
    public IResult GetRandomSudoku(int difficulty)
    {
        _logger.LogInformation("Http Get request for random sudoku // {0}", DateTime.UtcNow.ToString());
        try
        {
            var result = RandomSudoku.GetRandomSudoku(difficulty);
            return Results.Ok(result);
        } catch (Exception ex)
        {
            ModelState.AddModelError("Bad request", ex.Message);
            return Results.BadRequest(ModelState);
        }
    }
}
