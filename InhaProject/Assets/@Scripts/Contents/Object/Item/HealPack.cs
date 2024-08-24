using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class HealPack : BaseItem
{
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
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