using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : BaseItem
{
    private void OnTriggerEnter(Collider other)
    {


        if (!isPlayerInRange &&  other.CompareTag("Player") ) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            Debug.Log("�κ��丮��");
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlayerInRange && other.CompareTag("Player"))
            isPlayerInRange = false;
    }
    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.ManaPotion;

        return true;
    }
}
