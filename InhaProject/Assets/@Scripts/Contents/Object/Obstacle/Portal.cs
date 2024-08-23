using System.Collections.Generic;
using UnityEngine;

public class Portal : Obstacle
{
    [SerializeField]
    public Portal connectedPortal; 

    private static List<Portal> allPortals = new List<Portal>(); // 모든 포탈을 추적하는 리스트
        
    public override bool Init()
    {
        if (!base.Init())
            return false;
        allPortals.Add(this);

        // 포탈이 연결되지 않은 경우만 자동 연결 로직 적용
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

    private void TeleportPlayer(Player player)
    {
        if (connectedPortal != null)
        {
            player.transform.position = connectedPortal.transform.position;
        }

    }

}