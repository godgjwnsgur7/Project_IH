using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class HealPack : BaseItem
{
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
        }
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.HealPack;

        return true;
    }
}