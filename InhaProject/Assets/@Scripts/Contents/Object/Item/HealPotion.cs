using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : BaseItem, IInventoryItem
{
    public string Name 
    {
        get { return "HealPotion"; }
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
        set {  _param = value; }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
        Destroy(gameObject); 
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

		if (collision.collider.CompareTag("Player")) 
		{
			isPlayerInRange = true;
			Player player = collision.collider.gameObject.GetComponent<Player>();

			if (player != null)
			{
				//player.inventory.AddItem(this);
                OnPickup();
			}
		}
	}

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.HealPotion;

        return true;
    }
}
