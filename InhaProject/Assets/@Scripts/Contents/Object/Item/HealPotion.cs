using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : BaseItem, IInventoryItem
{
    public string Name 
    {
        get
        {
            return "HealPotion";
        }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }
    
    public int _count = 1;
    public int Count
    {
        get
        {
            return _count;
        }
        set 
        {
            _count = value;
        }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

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
