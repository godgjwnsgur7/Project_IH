using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viapix_PlayerParams;
using static Define;

public class HealPack : BaseItem
{

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
       
        if (other.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
        {
            Managers.Game.Player.OnGetApplyItme(param);
            Managers.Object.DespawnObject(gameObject);
        }
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.HealPack;
        param = new ApplyItemParam(true, heal);
        param.type = ItemType;
        return true;
    }

   
}