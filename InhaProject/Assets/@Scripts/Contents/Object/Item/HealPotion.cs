using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : BaseItem
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.HealPotion;

        return true;
    }
}
