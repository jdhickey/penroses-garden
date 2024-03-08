using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTilePlacement : MonoBehaviour
{
    // This should get a 3x3 neighborhood of colliders around gridPos. It must sort the 3x3 properly to represent relative positioning somehow.
    // Check placement rules which are their own functions. Edge rules and corner rules.
    // If valid, place at gridPos and update the values of the tiles in the neighborhood and currTile. Else return false.
    public bool PlaceTile(Vector3 gridPos, SquareTile currTile){
        SquareTile[, ,] currNeighborhood = new SquareTile[3, 3, 3]
        bool valid = false;

        return valid;
    }

    // Return true if the edge rules determine this is a valid placement. Else false.
    public bool CheckEdgeRules(SquareTile[,,] currNeighborhood, SquareTile currTile){
        bool valid = false;

        return valid;
    }

    // Return true if the corner rules determine this is a valid placement. Else false.
    public bool CheckCornerRules(SquareTile[,,] currNeighborhood, SquareTile currTile){
        bool valid = false;

        return valid;
    } 
}
