using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : PenroseTile
{
    public EmptyTile() : base()
    {
        InitializeTile();
    }

    public void InitializeTile()
    {
        smallAngle = 0;
    }
}
