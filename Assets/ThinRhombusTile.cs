using System.Collections;
using System.Collections.Generic;
public class ThinRhombusTile : PenroseTile
{
    public override int CanConnectWith(PenroseTile otherTile)
    {
        for (int i = 0; i < 4; i++){
            if(freeSide[i]){
                if(i < 2){
                    if(otherTile.freeSide[(-1)*(i-1)]){
                        return (-1)*(i-1);
                    }
                }
                else{
                    if(otherTile.freeSide[(i-3)*(-1)+2]){
                        return (i-3)*(-1)+2;
                    }
                }
            }
        }
        return 4;
        
    }

    public override void OrientWith(PenroseTile otherTile)
    {
        
    }
}