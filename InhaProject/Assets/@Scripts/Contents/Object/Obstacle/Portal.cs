using System.Collections.Generic;
using UnityEngine;

public class Portal : Obstacle
{
    [SerializeField]
    public Portal connectedPortal; 

    private static List<Portal> allPortals = new List<Portal>(); // ��� ��Ż�� �����ϴ� ����Ʈ
        
    public override bool Init()
    {
        if (!base.Init())
            return false;
        allPortals.Add(this);

        // ��Ż�� ������� ���� ��츸 �ڵ� ���� ���� ����
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