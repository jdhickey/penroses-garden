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
    }

    public override int CanConnectWith(PenroseTile otherTile)
    {
        for (int i = 0; i < 4; i++)
        {
            if (otherTile.freeSide[i] == true)
            {
                if (i < 2)
                {
                    if (otherTile.freeSide[(-1) * (i - 1)])
                    {
                        return (-1) * (i - 1);
                    }
                }
                else
                {
                    if (otherTile.freeSide[(i - 3) * (-1) + 2])
                    {
                        return (i - 3) * (-1) + 2;
                    }
                }
            }
        }
        return 4;
    }

    public override void OrientWith(PenroseTile otherTile)
    {
    }
}
