using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;


// ������ �������̶� �̸� ������
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
    public EItemType ItemType { get; set; } = EItemType.None; // ������ Ÿ��


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
        // templateID�� ���� ������ Ÿ�� �� ���� ����
        ItemType = (EItemType)templateID;
    }




    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
    
}


