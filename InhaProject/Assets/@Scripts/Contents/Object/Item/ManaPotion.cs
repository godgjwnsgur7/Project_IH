using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : BaseItem
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
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
