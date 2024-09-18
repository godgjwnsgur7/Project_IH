using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPack : BaseItem
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
        param = new ApplyItemParam(false, heal);
        param.type = ItemType;
        return true;
    }
}
