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
        Debug.Log("Trying to place tile.");
        //SquareTile[, ,] currNeighborhood = new SquareTile[3, 3, 3];
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gridPos, 1);
        foreach (Collider2D collider in colliders) {
            if (collider){
                SquareTile temp = collider.gameObject.GetComponent<SquareTile>();
                Debug.Log(temp.sides);
            }
        }
        SquareTile[, ,] currNeighborhood = OrganizeNeighborhood(colliders);
        bool valid = false;

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
        return currNeighborhood;
    }

    // Possible connections 0 <-> 1, 2 <-> 3, 4 <-> 5
    // Return true if the edge rules determine this is a valid placement. Else false.
    public bool CheckEdgeRules(SquareTile[,,] currNeighborhood, SquareTile currTile){
        bool valid = false;
        if (!initialTile){
            valid = true;
        }
        if (!valid){
            Debug.Log("Violates edge rules. Not Placing.");
        }

        return valid;
    }

    // Return true if the corner rules determine this is a valid placement. Else false.
    public bool CheckCornerRules(SquareTile[,,] currNeighborhood, SquareTile currTile){
        bool valid = false;
        if (!initialTile){
            valid = true;
        }
        if (!valid){
            Debug.Log("Violates corner rules. Not Placing.");
        }

        return valid;
    } 
}
