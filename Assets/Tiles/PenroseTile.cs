using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class PenroseTile : MonoBehaviour
{
    public enum TileType
    {
        ThinRhombus,
        ThickRhombus
    }

    public TileType tileType;
    public PenroseTile[] freeSide = new PenroseTile[4];
    public int[,] colorFence = new int[2,2]; 
    public float sideLength = 0.0f;
    public float smallAngle = 0.0f;
    /*
    color fence = [line 0 color, line 1 color] 
                  [line 0 fence, line 1 fence]
    */
    public int CanConnectWith(PenroseTile otherTile, int[] ignore){
        for (int i = 0; i < 4; i++){
            int tester = (-1) * (i - 1);
            if (i < 2 && otherTile.freeSide[tester]  == null && Array.IndexOf(ignore, i) == -1){
                //Debug.Log("Connection: " + tester);
                return tester;
            }
            tester = (i - 3) * (-1) + 2;
            if (i > 1 && otherTile.freeSide[tester]  == null && Array.IndexOf(ignore, i) == -1){
                //Debug.Log("Connection: " + tester);
                return tester;
            }
        }
        return 4;
    }

    public float GetOffset(PenroseTile adjacentTile){
        return sideLength/2 + adjacentTile.sideLength/2;
    }

    public Vector2 CalculatePositionOffset(PenroseTile adjacentTile, float rotationAngle, int connection)
    {
        float radians = Mathf.Deg2Rad * rotationAngle;
        float offset = 0.0f;
        offset = GetOffset(adjacentTile);
        float rotatedY = offset * Mathf.Cos(radians);
        float rotatedX = offset * Mathf.Sin(radians);
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            if(connection == 1){
                rotatedY *= -1;
            }
            else if(connection == 3){
                rotatedX *= -1;
            }
        }

        return new Vector2(rotatedX, rotatedY);
    }

    public float CalculateRotation(PenroseTile adjacentTile, int connection)
    {
        float adjacentRotation = adjacentTile.transform.rotation.eulerAngles.z;
        float rotationAngle = 0;
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            if(connection == 1 || connection == 3){
                rotationAngle = adjacentRotation + 216;
            }
            else if(connection == 2 || connection == 0){
                rotationAngle = adjacentRotation + 144;
            }
        }
        return rotationAngle;
    }

    public bool IsTouchingTile()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<PolygonCollider2D>().bounds.size, 0);
        
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider != GetComponent<Collider2D>())
            {
                PenroseTile otherTile = collider.gameObject.GetComponent<PenroseTile>();
                if (otherTile != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Awake(){
        freeSide = new PenroseTile[4];
        for (int i = 0; i < 4; i++){
            freeSide[i] = null;
        }

        sideLength = (GetComponent<PolygonCollider2D>().bounds.size).y;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null){
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }
    }
}