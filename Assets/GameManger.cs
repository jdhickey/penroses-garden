using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public ThinRhombusTile ThinRhombusPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlaceTile(mousePos);
        }
    }

    void PlaceTile(Vector2 position)
    {
        ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
        newTile.InitializeTile();
        int[] ignore = new int[0]; 
        OrientTile(newTile, ignore);
    }

    void OrientTile(ThinRhombusTile tile, int[] ignore)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 0.2f);
        
        foreach (Collider2D collider in colliders)
        {
            PenroseTile adjacentTile = collider.GetComponent<PenroseTile>();

            if (adjacentTile != null && adjacentTile != tile)
            {
                
                int connection = tile.CanConnectWith(adjacentTile, ignore);
                if (connection != 4)
                {
                    float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                    tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                    
                    Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                    Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                    tile.transform.position = newPosition;

                    Collider2D[] collisionCheck = Physics2D.OverlapCircleAll(tile.transform.position, 0.2f);
                    if(collisionCheck.Length != 0){
                        //ADD THE CURRENT NODE TO IGNORE BEFORE RETRYING
                        OrientTile(tile, ignore);
                    }
                    else{
                        adjacentTile.freeSide[connection] = false;
                        int i = 0;
                        if (connection < 2){
                            i = (-1) * (connection - 1);
                        }
                        else{
                            i = (connection - 3) * (-1) + 2;
                        }
                        tile.freeSide[i] = false;
                    }
                    return;
                }
            }
        }
    }
    
    
}
