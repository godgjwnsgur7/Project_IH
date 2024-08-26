using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPack : BaseItem
{
  

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.ManaPack;

        return true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
