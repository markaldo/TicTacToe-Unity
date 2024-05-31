using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public int Row { get; set; }
    public int Col { get; set; }

    public Move(int row, int col)
    {
        Row = row;
        Col = col;
    }
}
