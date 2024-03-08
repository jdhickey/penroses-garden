using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinRhombusTile : PenroseTile
{

    public ThinRhombusTile() : base()
    {
        InitializeTile();
    }

    public override void InitializeTile()
    {
        smallAngle = 36;
        tileType = TileType.ThinRhombus;
    }
}
