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

    // Takes in the tile that another tile is trying to connect with and ignore(?)
    // Returns the value of the first side it can connect to.
    // Used in PlayerTilePlacement.cs
    public int CanConnectWith(PenroseTile otherTile, int[] ignore){
        int tester; // adjcaentSide is the side on otherTile that the instantiated tile should connect with.
        
        for (int i = 0; i < 4; i++){

            tester = (-1) * (i - 1); // When i = 0, tester = 1. When i = 1, tester = 0.
            // If the appropriate side is empty and the current i value is not in ignore.
            if (i < 2 && otherTile.freeSide[tester]  == null && Array.IndexOf(ignore, i) == -1){
                //Debug.Log("Connection: " + tester);
                return tester;
            }

            tester = (i - 3) * (-1) + 2; // When i = 2, tester = 3. When i = 3, tester = 2.
            // If the appropriate side is empty and the current i value is not in ignore.
            if (i > 1 && otherTile.freeSide[tester]  == null && Array.IndexOf(ignore, i) == -1){
                //Debug.Log("Connection: " + tester);
                return tester;
            }
        }

        // If no connections could be made, return 4
        return 4;
    }  // Rework to swap tester and i for clarity?
    
    // Takes in an adjacent tile to be tested and the side on adjacentTile to calculate the necessary rotation for.
    // Returns the rotation angle for the tile it's called upon.
    // Used in PlayerTilePlacement.cs in OrientTile.
    public float CalculateRotation(PenroseTile adjacentTile, int connection)
    {
        float adjacentRotation = adjacentTile.transform.rotation.eulerAngles.z; // Rotation of adjacentTile in the 2d plane.
        float rotationAngle = 0;
        
        // If the adjacent tile is thin rather than thick.
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            // If it's connecting to side 1 or 2 on adjacentTile
            if(connection == 1 || connection == 2){
                rotationAngle = adjacentRotation + 216;
            }
            // If it's connecting to side 0 or 3 on adjacentTile.
            else if(connection == 0 || connection == 3){
                rotationAngle = adjacentRotation + 144;
            }
        }

        return rotationAngle % 360; // Always returns the appropriate rotation angle.
    }

    // Takes in the adjacentTile being considered, the angle of rotation that the current tile needs and the side on adjacentTile that is being connected to.
    // Returns a vector that represents the direction that the tile needs to be translated.
    // Used in PlayerTilePlacement.cs
    public Vector2 CalculatePositionOffset(PenroseTile adjacentTile, float rotationAngle, int connection)
    {
        float rotationRadians = Mathf.Deg2Rad * rotationAngle; // rotationAngle in radians.
        float offset = GetOffset(adjacentTile); // Half the side length of each tile added together.

        float rotatedY = 0;
        float rotatedX = 0;

        // If the adjacentTile is thin.
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            if(connection == 0){
                rotatedX = 0;
                rotatedY = offset;
            }
            else if(connection == 1){
                rotatedY = -1 * offset * Mathf.Cos(rotationRadians);
                rotatedX = offset * Mathf.Sin(rotationRadians);
            }
            else if(connection == 2){
                rotatedX = -1 * offset * Mathf.Sin(rotationRadians);
                rotatedY = offset * Mathf.Cos(rotationRadians);
            }
            else if(connection == 3){
                rotatedX = 0;
                rotatedY = -1 * offset;
            }
        }

        return new Vector2(rotatedX, rotatedY);
    }

    // Takes in the adjacentTile.
    // Returns half the side length of each, added together.
    public float GetOffset(PenroseTile adjacentTile){
        return sideLength / 2 + adjacentTile.sideLength / 2;
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

    public abstract void InitializeTile();
    
    }