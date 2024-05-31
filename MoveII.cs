using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct MoveII
{
    public int Index { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public MoveII(int index, int row, int col)
    {
        Index = index;
        Row = row;
        Col = col;
    }

    // Deconstruct method
    public void Deconstruct(out int index, out int row, out int col)
    {
        index = Index;
        row = Row;
        col = Col;
    }
}

