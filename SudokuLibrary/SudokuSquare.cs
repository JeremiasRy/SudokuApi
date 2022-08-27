namespace SudokuLibrary;

public class SudokuSquare
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Box { get; private set; }
    public Values Value { get; private set; }
    public List<SudokuSquare> SudokuTable { get; set; }

    public bool IsCorrect { get => (HorizontalCheck(Value) && VerticalCheck(Value) && BoxCheck(Value)); }

    bool HorizontalCheck(Values value) => !SudokuTable.Any(SudSquare => SudSquare.X == X && SudSquare != this && SudSquare.Value == value);
    bool VerticalCheck(Values value) => !SudokuTable.Any(SudSquare => SudSquare.Y == Y && SudSquare != this && SudSquare.Value == value);
    bool BoxCheck(Values value) => !SudokuTable.Any(SudSquare => SudSquare.Box == Box && SudSquare != this && SudSquare.Value == value);

    public List<Values> PossibleCorrectValues()
    {
        var posValues = new List<Values>();
        if (!Value.Equals(Values.NoValue)) 
            return posValues;
        foreach(Values value in Enum.GetValues(typeof(Values))) 
        {
            if (value.Equals(Values.NoValue))
                continue;
            if(HorizontalCheck(value) && VerticalCheck(value) && BoxCheck(value))
            {
                posValues.Add(value);
            }   
        }
        return posValues;
    }
    public void InsertValue(Values value) => Value = value;
    public SudokuSquare(Values value, int x, int y, int box, List<SudokuSquare> sudokuTable)
    {
        X = x;
        Y = y;
        Value = value;
        Box = box;
        SudokuTable = sudokuTable;
    }
}
