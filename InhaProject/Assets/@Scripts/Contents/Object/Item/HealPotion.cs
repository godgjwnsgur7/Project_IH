using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : BaseItem
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            //heal �ڵ�
        }
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.HealPotion;

        return true;
    }
}