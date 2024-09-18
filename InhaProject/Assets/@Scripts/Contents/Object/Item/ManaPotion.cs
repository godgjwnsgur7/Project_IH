using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : BaseItem, IInventoryItem
{
    
    public string Name
    {
        get { return "ManaPotion"; }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get { return _Image; }
    }

    public int _count = 1;

    public int Count
    {
        get { return _count; }
        set { _count = value; }
    }

    ItemParam _param;
    public ItemParam Param
    {
        get { return _param; }
        set { _param = value; }
    }

    public void OnPickup()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Player"))
        {
            Managers.Game.Player.OnGetInventroyItem(this);
            Managers.Object.DespawnObject(gameObject);
        }
        
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.ManaPotion;
        _param = new PotionItemParam(false, heal);
        _param.type = ItemType;
        return true;
    }



}
