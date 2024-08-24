using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : Obstacle
{
    [SerializeField]
    public Portal connectedPortal; 


    private static List<Portal> allPortals = new List<Portal>(); // 모든 포탈을 추적하는 리스트  -> 필요할까? 없을듯?
        
    public override bool Init()
    {
        if (!base.Init())
            return false;

        obstacleType = EObstacleType.Portal;
        allPortals.Add(this);

        // 포탈이 연결되지 않은 경우만 자동 연결 로직 적용 ->  필요할까? 없을듯?
        if (connectedPortal == null && allPortals.Count > 1)
        {
            foreach (Portal portal in allPortals)
            {
                if (portal != this && portal.connectedPortal == null)
                {
                    connectedPortal = portal;
                    portal.connectedPortal = this;
                    break;
                }
            }
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; 
            player ??= other.gameObject.GetComponent<Player>();
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