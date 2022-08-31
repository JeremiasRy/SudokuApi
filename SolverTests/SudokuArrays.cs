using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public static class SudokuArrays
{
    public static readonly int[] sudCorrect = new int[81]
    {
        1,2,3,4,5,6,7,8,9,4,5,6,7,8,9,1,2,3,7,8,9,1,2,3,4,5,6,2,3,4,5,6,7,8,9,1,5,6,7,8,9,1,2,3,4,8,9,1,2,3,4,5,6,7,3,4,5,6,7,8,9,1,2,6,7,8,9,1,2,3,4,5,9,1,2,3,4,5,6,7,8
    };
    public static readonly int[] sudCorrectNineEmpty = new int[81]
    {
        1,2,3,4,5,6,0,0,0,4,5,6,7,8,9,1,2,3,0,0,0,1,2,3,4,5,6,2,3,4,5,6,7,8,9,1,5,6,7,0,0,0,2,3,4,8,9,1,2,3,4,5,6,7,3,4,5,6,7,8,9,1,2,6,7,8,9,1,2,3,4,5,9,1,2,3,4,5,6,7,8
    };
    public static readonly int[] sudHard = new int[81]
    {
        0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,2,0,0,0,8,6,0,0,0,5,1,0,0,0,9,1,0,2,7,0,0,0,2,0,0,3,0,0,8,0,0,7,0,0,0,0,0,5,0,6,0,0,0,2,0,0,0,9,0,0,2,5,0,4,6,0,0,9,0,0,8,0,6,0,0,5
    };
    public static readonly int[] sudHardAlmostEmpty = new int[81]
    {
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,9,0,0,8,0,6,0,0,5
    };
    public static readonly int[] sudImpossible = new int[81]
    {
        1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,9,0,0,8,0,6,0,0,5
    };
    public static int[] Empty()
    {
        int[] sud = new int[81];
        for (int i = 0; i < sud.Length; i++)
        {
            sud[i] = 0;
        }
        return sud;
    } 
}
