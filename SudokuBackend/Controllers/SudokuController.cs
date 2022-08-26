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
    [HttpGet(Name = "GetARandomSudoku")]
    public IResult GetSudoku()
    {
        Random random = new Random();   
        List<int> result = new List<int>();

        _logger.LogInformation("HTTP GET // {0}", DateTime.UtcNow);
        for (int i = 0; i < 100; i++)
        {
            result.Add(random.Next(0, 9999));
        }
        return Results.Ok(result);
    }
    [HttpPost(Name = "SolveASudoku")]
    public IResult SolveSudoku(int[] sudoku)
    {
        if (sudoku.Length != 81)
        {
            return Results.Ok("Sudoku is in incorrect format");
        }

        _logger.LogInformation("HTTP POST // {0}", DateTime.UtcNow);
        return Results.Ok(sudoku.ToList());
    }
}
