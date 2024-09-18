using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : Obstacle
{
    [SerializeField]
    public Portal connectedPortal; 


   
    public override bool Init()
    {
        if (!base.Init())
            return false;

        obstacleType = EObstacleType.Portal;
    

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; 
            player ??= other.gameObject.GetComponent<Player>();
            if(connectedPortal != null)
                TeleportPlayer(connectedPortal);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public override void TeleportPlayer(Portal otherPortal = default)
    {
        base.TeleportPlayer(connectedPortal);
    }
}