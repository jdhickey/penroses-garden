using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinRhombusTile : PenroseTile
{

    public ThinRhombusTile() : base()
    {
        InitializeTile();
    }

    public new void InitializeTile()
    {
        smallAngle = 36;
    }
}
