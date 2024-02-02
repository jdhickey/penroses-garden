using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinRhombusTile : PenroseTile
{

    public ThinRhombusTile() : base()
    {
        InitializeTile();
    }

    public void InitializeTile()
    {
        for (int i = 0; i < 4; i++)
        {
            freeSide[i] = true;
        }
        smallAngle = 36;
    }
}
