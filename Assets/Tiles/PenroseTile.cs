using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public float sideLength = 0.0f;
    public float smallAngle = 0.0f;
    /*
    color fence = [line 0 color, line 1 color] 
                  [line 0 fence, line 1 fence]
    */
    public int CanConnectWith(PenroseTile otherTile, int[] ignore)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < 2 && otherTile.freeSide[(-1) * (i - 1)] && !ignore.Contains(i))
            {
                return (-1) * (i - 1);
            }
            else if (otherTile.freeSide[(i - 3) * (-1) + 2])
            {
                return (i - 3) * (-1) + 2;
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
        float offsetX = 0.0f;
        float offsetY = 0.0f;
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            offsetX = GetOffset(adjacentTile);
            offsetY = GetOffset(adjacentTile) * (Mathf.Sin(Mathf.Deg2Rad * 36)/Mathf.Sin(Mathf.Deg2Rad * 56));
            Debug.Log("connection:" + connection);
            if(connection == 0){
                offsetX *= -1;
            }
            else if(connection == 1){
                offsetX *= 1;
            }
            else if(connection == 2){
                offsetY *= -1;
            }
            else if(connection == 3){
                offsetX *= -1;
                offsetY *= -1;
            }
        }

        Debug.Log("Pre rotation:" + offsetX + " " + offsetY);
        float rotatedY = offsetX * Mathf.Cos(radians) - offsetY * Mathf.Sin(radians);
        float rotatedX = offsetX * Mathf.Sin(radians) + offsetY * Mathf.Cos(radians);
        Debug.Log("Post rotation:" + rotatedX + " " + rotatedY);

        return new Vector2(rotatedX, rotatedY);
    }

    public float CalculateRotation(PenroseTile adjacentTile, int connection)
    {
        float adjacentRotation = adjacentTile.transform.rotation.eulerAngles.z;
        float rotationAngle = 0;
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            if(connection == 0 || connection == 2){
                rotationAngle = adjacentRotation + 216;
            }
            else if(connection == 1 || connection == 3){
                rotationAngle = adjacentRotation + 144;
            }
        }
        return rotationAngle;
    }

    private void OnCollision(Collision2D collision)
    {
        PenroseTile otherTile = collision.gameObject.GetComponent<PenroseTile>();
        if (otherTile != null)
        {
            Debug.Log("Collision with another PenroseTile detected!");
        }
    }

    public void Start()
    {
        sideLength = (GetComponent<PolygonCollider2D>().bounds.size).y;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }
    }
}