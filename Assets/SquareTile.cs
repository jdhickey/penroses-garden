using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTile : MonoBehaviour
{
    public int[] sides = new int[4];
    public bool corners = false;
    public SquareTile[] neigh = new SquareTile[4];

    public void rotateSides(bool dir){ // dir true = right
        if(dir){
            int temp = sides[0];
            int temp2 = sides[1];
            sides[0] = sides[3];
            sides[1] = temp;
            temp = sides[2];
            sides[2] = temp2;
            sides[3] = temp;
        }
        else{
            int temp = sides[3];
            int temp2 = sides[2];
            sides[3] = sides[0];
            sides[2] = temp;
            temp = sides[1];
            sides[1] = temp2;
            sides[0] = temp;
        }

    }
}
