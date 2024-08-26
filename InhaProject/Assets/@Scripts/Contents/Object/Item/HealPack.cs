using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viapix_PlayerParams;
using static Define;

public class HealPack : BaseItem
{
 


    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.HealPack;

        return true;
    }

    
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}