using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public GameObject gameManager;

    public Vector3 currPos = new Vector3(0, 0, 0);

    public ThinRhombusTile ThinRhombusPrefab;

    public bool temporaryPlacedTile = false;

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
        if(temporaryPlacedTile == true){
            ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
            newTile.InitializeTile();
            int[] ignore = {4,4,4,4};
            OrientTile(newTile, ignore);
        }
        else{
            ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
            newTile.InitializeTile();
            temporaryPlacedTile = true;
        }
    }

    bool InitializePosition(PenroseTile tile, int[] ignore){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, 0.01f);
        float n = 0.01f;
        while(colliders.Length == 1){
            colliders = Physics2D.OverlapCircleAll(tile.transform.position, n);
            n += 0.01f;
            if (n >= tile.sideLength*2){
                return false; 
            }
        }
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();
        foreach (Collider2D collider in colliders){
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                if (temp != null && temp != tile){
                    adjacentTiles.Add(temp);
                }
            }
        }
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        for (int i = 0; i < adjacentTiles.Count; i++){
            PenroseTile adjacentTile = adjacentTiles[i];
            if (adjacentTile != null && adjacentTile != tile){
                float xOff = Simplify(x - adjacentTile.transform.position.x);
                float yOff = Simplify(y - adjacentTile.transform.position.y);
                Debug.Log("Old offset: " + xOff + ", " + yOff);
                for(int j = 0; j < 4; j++){
                    Debug.Log(ignore[0] + " " + ignore[1] + " " + ignore[2] + " " + ignore[3]);
                    int connection = tile.CanConnectWith(adjacentTile, ignore);
                    if (connection != 4){
                        float rotationAngle = tile.CalculateRotation(adjacentTile, connection);
                        tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                        Vector2 offset = tile.CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                        Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                        tile.transform.position = newPosition;
                        Debug.Log("New offset: " + Simplify(tile.transform.position.x - adjacentTile.transform.position.x) + ", " + Simplify(tile.transform.position.y - adjacentTile.transform.position.y));
                        if(xOff == Simplify(tile.transform.position.x - adjacentTile.transform.position.x) && yOff == Simplify(tile.transform.position.y - adjacentTile.transform.position.y)){
                            j = adjacentTiles.Count;
                            Debug.Log("Correct Position");
                            return true;
                        }
                        else{
                            ignore[j] = j;

                        }
                    }
                    else{
                        return false;
                    }
                }
            }
        }   
        return false;
    }

    public float Simplify(float n){
        if(n >= 0){
            n = 1;
        }
        else{
            n = -1;
        }
        return n;
    }

    private bool IsValidPlacement(PenroseTile tile, PenroseTile adjacentTile, int connection){
        if (tile.IsTouchingTile()){
            Debug.Log("Overlap detected");
            return true;
        }
        if ((connection <= 1 && tile.freeSide[(-1) * (connection - 1)] == null) || (connection >= 1 && tile.freeSide[(connection - 3) * (-1) + 2] == null)){
            Debug.Log("Valid connection");
            return true;
        }
        Debug.Log("Invalid connection");
        return false;
    }


    void OrientTile(PenroseTile tile, int[] ignore){
        if (Array.IndexOf(ignore, 4) == -1 || !InitializePosition(tile, ignore)){
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, tile.sideLength * 0.31f);
        List<PenroseTile> adjacentTiles = new List<PenroseTile>();
        foreach (Collider2D collider in colliders){
            if (collider != null){
                PenroseTile temp = collider.gameObject.GetComponent<PenroseTile>();
                if (temp != null && temp != tile){
                    adjacentTiles.Add(temp);
                }
            }
        }
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        float r = tile.transform.rotation.eulerAngles.z;
        bool validPlacement = true;
        List<int> connections = new List<int>();
        for (int i = 0; i < adjacentTiles.Count; i++){
            if(adjacentTiles[i] != null){
                if(adjacentTiles[i]){
                    int connection = tile.CanConnectWith(adjacentTiles[i], ignore);
                    if (connection == 4 || !IsValidPlacement(tile, adjacentTiles[i], connection)){
                        validPlacement = false;
                    }
                    else{
                        //Debug.Log("Ignoring:" + ignore);
                        connections[i] = connection;
                    }
                }
            }
        }
        if(validPlacement){
            for (int i = 0; i < adjacentTiles.Count; i++){
                adjacentTiles[i].freeSide[connections[i]] = tile;
                if(connections[i] <= 1){
                    tile.freeSide[(-1) * (connections[i] - 1)] = adjacentTiles[i];
                    Debug.Log("Tile connected with side:" + ((-1) * (connections[i] - 1)));
                }
                else{
                    tile.freeSide[(connections[i] - 3) * (-1) + 2] = adjacentTiles[i];
                    Debug.Log("Tile connected with side:" + ((connections[i] - 3) * (-1) + 2));
                }
            }
        }
        else{
            Destroy(tile.gameObject);
            Debug.Log("No Tile Placed");
            return;
        }
    }
}
