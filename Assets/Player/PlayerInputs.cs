using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public GameObject gameManager;

    public Vector3 currPos = new Vector3(0, 0, 0);

    public ThinRhombusTile ThinRhombusPrefab;

    void OnShuffle()
    {
        print("Shuffle!");
    }

    void OnInventorySelect()
    {
        print("Inventory Select!");
    }

    void OnInventoryScroll()
    {
        print("Inventory Scroll!");
    }

    void OnExit()
    {
        print("Exit!");
    }

    void OnPlace()
    {
        currPos.x = transform.position.x;
        currPos.y = transform.position.y;
        PlaceTile(currPos);
    }

    void PlaceTile(Vector2 position){
        ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
        newTile.InitializeTile();
        int[] ignore = {4,4,4,4};
        OrientTile(newTile, ignore);
    }

    void InitializePosition(PenroseTile tile){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 0.01f);
        float n = 0.01f;
        while(colliders.Length == 1 ){
            colliders = Physics2D.OverlapCircleAll(tile.transform.position, n);
            n += 0.01f;
            if (n == tile.sideLength*2){
                Debug.Log("No Collision detected");   
                break; 
            }
        }
        Debug.Log("Collision detected");
    }

    void OrientTile(PenroseTile tile, int[] ignore){
        if (Array.IndexOf(ignore, 4) == -1){
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
        InitializePosition(tile);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, tile.sideLength * 0.3f);
        List<GameObject> adjacentTiles = new List<GameObject>();
        foreach (Collider2D collider in colliders){
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                if (temp != null){
                    adjacentTiles.Add(collider.gameObject);
                }
            }
        }
        Debug.Log("adjacentTiles length:" + adjacentTiles.Count);

        foreach (GameObject adjacent in adjacentTiles){

        }
        /*
            PenroseTile adjacentTile = collider.GetComponent<PenroseTile>();
            if (adjacentTile != null && adjacentTile != tile){
                int connection = tile.CanConnectWith(adjacentTile, ignore);
                if (connection != 4){
                    float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                    tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                    Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                    Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                    tile.transform.position = newPosition;
                    
                    PenroseTile otherTile = tile.collision.gameObject.GetComponent<PenroseTile>();
                    if (otherTile == null){
                        for (int i = 0; i <= 4; i++){
                            if (ignore[i] == 4){
                                ignore[i] = connection;
                                break;
                            }
                        }
                        OrientTile(tile, ignore);
                    }
        
                    else
                    {
                        adjacentTile.freeSide[connection] = tile;
                        int i = 0;
                        if (connection < 2)
                        {
                            i = (-1) * (connection - 1);
                        }
                        else
                        {
                            i = (connection - 3) * (-1) + 2;
                        }
                        tile.freeSide[i] = adjacentTile;
                    }
                    Debug.Log("Tile Placed");
                    return;
                }
            }
            */
        
    }
}
