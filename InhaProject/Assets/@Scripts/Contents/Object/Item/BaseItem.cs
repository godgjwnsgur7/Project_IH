using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;


// 아이템 프리펩이랑 이름 같을것
public enum EItemType
{
    None,
    ItemBox,
    HealPotion,
    ManaPotion,
    HealPack,
    ManaPack,

    Key,

    Max
}

public abstract class BaseItem : BaseObject
{
    public EItemType ItemType { get; set; } = EItemType.None; // 아이템 타입


    protected bool isPlayerInRange = false;

    protected ItemParam param;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        
        SetInfo();
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);
        // templateID에 따라 아이템 타입 및 상태 설정
        ItemType = (EItemType)templateID;
    }




    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
    
}


