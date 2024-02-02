using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PenroseTile : MonoBehaviour
{
    public enum TileType
    {
        ThinRhombus,
        ThickRhombus
    }

    public TileType tileType;
    public bool[] freeSide = new bool[4];
    public int[,] colorFence = new int[2,2]; 
    /*
    color fence = [line 0 color, line 1 color] 
                  [line 0 fence, line 1 fence]
    */

    public abstract int CanConnectWith(PenroseTile otherTile);
    public abstract void OrientWith(PenroseTile otherTile);
}