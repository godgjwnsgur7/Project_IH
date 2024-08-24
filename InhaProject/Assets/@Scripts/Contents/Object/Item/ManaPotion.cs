using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : BaseItem
{
    private void OnTriggerEnter(Collider other)
    {


        if (!isPlayerInRange &&  other.CompareTag("Player") ) // 충돌한 객체가 Player 태그를 가진 경우
        {
            Debug.Log("인벤토리로");
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
