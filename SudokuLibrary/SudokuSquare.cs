using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary;

public class SudokuSquare
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Box { get; set; }
    public int Value { get; set; } // 0 means no value

    public bool HasValue { get { return Value != 0; } }

    public SudokuSquare(int value, int x, int y, int box)
    {
        X = x;
        Y = y;
        Value = value;
        Box = box;
    }
}
