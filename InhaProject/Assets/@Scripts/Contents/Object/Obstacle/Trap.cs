using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Obstacle
{
    // Start is called before the first frame update
    public override bool Init()
    {
        if (!base.Init())
            return false;

        obstacleType = EObstacleType.Trap;


        return true;
    }

    
}
