using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickRhombusTile : PenroseTile
{

    public ThickRhombusTile() : base()
    {
        InitializeTile();
    }

    public void InitializeTile()
    {
        smallAngle = 72; // We've gotta update this number.
    }
}
