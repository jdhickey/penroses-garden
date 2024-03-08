using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickRhombusTile : PenroseTile
{

    public ThickRhombusTile() : base()
    {
        InitializeTile();
    }

    public override void InitializeTile()
    {
        smallAngle = 72;
        tileType = TileType.ThickRhombus;
    }
}
