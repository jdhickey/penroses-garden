using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTilePlacement : MonoBehaviour
{
    public bool initialTile = false;

    // This should get a 3x3 neighborhood of colliders around gridPos. It must sort the 3x3 properly to represent relative positioning somehow.
    // Check placement rules which are their own functions. Edge rules and corner rules.
    // If valid, place at gridPos and update the values of the tiles in the neighborhood and currTile. Else return false.
    public bool PlaceTile(Vector3 gridPos, SquareTile currTile){
        SquareTile[,] currNeighborhood = new SquareTile[3, 3];
        bool valid = false;
        Vector3 snapPos = Snap(gridPos);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(snapPos, 1.0f);
        foreach (Collider2D collider in colliders) {
            if (collider != null){
                SquareTile tile = collider.gameObject.GetComponent<SquareTile>();
                Vector3 tilePos = Snap(tile.transform.position);
                currNeighborhood = addToNeighborhood(currNeighborhood, snapPos, tilePos, tile);
            }
        }
        if(CheckEdgeRules(currNeighborhood, currTile) && CheckCornerRules(currNeighborhood, currTile)){
            valid = true;
        }

        //Place, Orient and adjust the neigh variable for the (up to) 5 tiles

        valid = CheckCornerRules(currNeighborhood, currTile);
        if (valid){
            valid = CheckEdgeRules(currNeighborhood, currTile);
            if (valid){
                Quaternion currTileRotation = Quaternion.identity;
                currTileRotation.eulerAngles = new Vector3(0, 0, currTile.rotation);
                Debug.Log(currTileRotation.eulerAngles);
                SquareTile newTile = Instantiate(currTile, gridPos, currTileRotation);
                if (!initialTile){
                    initialTile = true;
                }
            }
        }

        return valid;
    }

    // This function will organize the colliders properly.
    public SquareTile[,,] OrganizeNeighborhood(Collider2D[] colliders){
        SquareTile[, ,] currNeighborhood = new SquareTile[3, 3, 3];

        // Insert organizing code here.

        return currNeighborhood;
    }

    // Possible connections 0 <-> 1, 2 <-> 3, 4 <-> 5
    // Return true if the edge rules determine this is a valid placement. Else false.
    public bool CheckEdgeRules(SquareTile[,] currNeighborhood, SquareTile currTile){
        bool valid = true;
        if(currNeighborhood[1,0] != null){ // Testing side 0
            if(!CompareSides(currTile.sides[0], currNeighborhood[1,0].sides[2])){
                valid = false;
            }
        }
        if(currNeighborhood[2,1] != null){ // Testing side 1
            if(!CompareSides(currTile.sides[1], currNeighborhood[2,1].sides[3])){
                valid = false;
            }
        }
        if(currNeighborhood[1,2] != null){ // Testing side 2
            if(!CompareSides(currTile.sides[2], currNeighborhood[1,2].sides[0])){
                valid = false;
            }
        }
        if(currNeighborhood[0,1] != null){// Testing side 3
            if(!CompareSides(currTile.sides[3], currNeighborhood[0,1].sides[1])){
                valid = false;
            }
        }
        return valid;
    }

    // Return true if the 2 sides can interlock . Else false.
    public bool CompareSides(int currTile, int otherTile){
        bool valid = false;
        if((currTile == 0 && otherTile == 1) || (currTile == 1 && otherTile == 0) || (currTile == 2 && otherTile == 3) || (currTile == 3 && otherTile == 2) || (currTile == 4 && otherTile == 5) || (currTile == 5 && otherTile == 4)){
            valid = true;
        }
        return valid;
    }

    // Return true if the corner rules determine this is a valid placement. Else false.
    public bool CheckCornerRules(SquareTile[,] currNeighborhood, SquareTile currTile){
        bool valid = true;
        for(int i = 0; i < 2; i++){
            for(int j = 0; j < 2; j++){
                int cornerCount = 0;
                int tiles = 0;
                for(int k = 0; k < 2; k++){
                    for(int l = 0; l < 2; l++){
                        if(currNeighborhood[i+k, j+l] != null){
                            tiles++;
                            if(currNeighborhood[i+k, j+l].corners){
                                cornerCount++;
                            }
                        }
                    }
                }
                if(cornerCount != 1 && tiles == 3){
                    valid = false;
                }
            }
        }
        return valid;
    } 

    // Return the same vector rounded to the nearest whole coordinate.
    public Vector3 Snap(Vector3 vector3){
        float grid = 1.0f;
        return new Vector3(
            Mathf.Round(vector3.x / grid) * grid,
            Mathf.Round(vector3.y / grid) * grid,
            Mathf.Round(vector3.z / grid) * grid);
    }

    // Returns the same array but with the new tile added to the correct position
    public SquareTile[,] addToNeighborhood(SquareTile[,] currNeighborhood, Vector3 snapPos, Vector3 tilePos, SquareTile tile){
        if(tilePos.x > snapPos.x){
            if(tilePos.y > snapPos.y){
                currNeighborhood[0,0] = tile;
            }
            else if(tilePos.y == snapPos.y){
                currNeighborhood[0,1] = tile;
            }
            else if(tilePos.y < snapPos.y){
                currNeighborhood[0,2] = tile;
            }
        }
        else if(tilePos.x == snapPos.x){
            if(tilePos.y > snapPos.y){
                currNeighborhood[1,0] = tile;
            }
            else if(tilePos.y == snapPos.y){
                currNeighborhood[1,1] = tile;
            }
            else if(tilePos.y < snapPos.y){
                currNeighborhood[1,2] = tile;
            }
        }
        else if(tilePos.x == snapPos.x){
            if(tilePos.y > snapPos.y){
                currNeighborhood[2,0] = tile;
            }
            else if(tilePos.y == snapPos.y){
                currNeighborhood[2,1] = tile;
            }
            else if(tilePos.y < snapPos.y){
                currNeighborhood[2,2] = tile;
            }
        }
        return currNeighborhood;
    }
}
