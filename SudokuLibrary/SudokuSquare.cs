namespace SudokuLibrary;
public class SudokuSquare
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Box { get; private set; }
    public Values Value { get; private set; }
    readonly List<SudokuSquare> _referenceTable;

    public bool IsCorrect => HorizontalCheck(Value) && VerticalCheck(Value) && BoxCheck(Value) && Value != Values.NoValue;

    bool HorizontalCheck(Values value) => !_referenceTable.Any(SudSquare => SudSquare.X == X && SudSquare != this && SudSquare.Value == value);
    bool VerticalCheck(Values value) => !_referenceTable.Any(SudSquare => SudSquare.Y == Y && SudSquare != this && SudSquare.Value == value);
    bool BoxCheck(Values value) => !_referenceTable.Any(SudSquare => SudSquare.Box == Box && SudSquare != this && SudSquare.Value == value);

    public List<Values> PossibleCorrectValues { get {

            var posValues = new List<Values>();
            if (Value != Values.NoValue)
                return posValues;
            foreach (Values value in Enum.GetValues(typeof(Values)))
            {
                if (value == Values.NoValue)
                    continue;
                if (HorizontalCheck(value) && VerticalCheck(value) && BoxCheck(value))
                {
                    posValues.Add(value);
                }
            }
            if (posValues.Any())
                return posValues;
            else
            { 
                posValues.Add(Values.NoValue);
                return posValues;
            }
        } }
    
    public void InsertValue(Values value) => Value = value;
    public SudokuSquare(Values value, int x, int y, int box, List<SudokuSquare> sudokuTable)
    {
        X = x;
        Y = y;
        Value = value;
        Box = box;
        _referenceTable = sudokuTable;
    }
}
