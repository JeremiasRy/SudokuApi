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

    [HttpGet("/solve/{sudoku}")]
    public async Task<IActionResult> GetCompletedSudoku(string sudoku)
    {
        try
        {
            int[] sudokuArray = sudoku.Split(",").Select(Int32.Parse).ToArray();
            try
            {
                var sudokuTable = await SudokuTable.BuildTable(sudokuArray);
                return Ok(sudokuTable.GetCompletedSudoku());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Bad request", ex.Message);
                return BadRequest(ModelState);
            }

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Wrong format", ex.Message);
            return BadRequest(ModelState);
        }
    }
    [HttpGet("/solveSquare/{x}/{y}/{sudoku}")]
    public async Task<IActionResult> GetOneCorrectSquare(string x,string y, string sudoku)
    {
        _logger.LogInformation("Http Get request to solve one square // {0}", DateTime.UtcNow.ToString());
        
        try
        {
            int[] sudokuArray = sudoku.Split(",").Select(Int32.Parse).ToArray();
            if (int.TryParse(x, out int xCoordinate) && int.TryParse(y, out int yCoordinate))
            {
                try
                {
                    SudokuTable sudokuTable = await SudokuTable.BuildTable(sudokuArray);
                    return Ok(sudokuTable.GetOneCorrectValue(xCoordinate, yCoordinate));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Bad request", ex.Message);
                    return BadRequest(ModelState);
                }
            } else
            {
                ModelState.AddModelError("Coordinates error", $"One or both of coordinates is not a number x: {x}, y: {y}");
                return BadRequest(ModelState);
            }
        } catch (Exception ex)
        {
            ModelState.AddModelError("Sudoku format", ex.Message);
            return BadRequest(ModelState);
        }

    }
    [HttpGet("/randomsudoku/{difficulty}")]
    public async Task<IActionResult> GetRandomSudoku(int difficulty)
    {
        _logger.LogInformation("Http Get request for random sudoku // {0}", DateTime.UtcNow.ToString());
        try
        {
            var result = await RandomSudoku.GetRandomSudoku(difficulty);
            return Ok(result);
        } catch (Exception ex)
        {
            ModelState.AddModelError("Bad request", ex.Message);
            return BadRequest(ModelState);
        }
    }
}
