using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemData
{
	public string name;
	public int count;
	public ItemParam param;

	public InventoryItemData(string name, int count, ItemParam param)
	{
		this.name = name;
		this.count = count;
		this.param = param;
	}
}

public class Inventory : MonoBehaviour
{
	private const int SLOT_SIZE = 6;

	private List<IInventoryItem> items = new List<IInventoryItem>();
 
	public event EventHandler<InventoryEventArgs> ItemAdd;
	public event EventHandler<InventoryEventArgs> ItemRemove;

    public IInventoryItem FindItem(string name)	
	{
		if (items.Exists(x => x.Name.Equals(name)))
		{
			IInventoryItem findItem = items.Find(x => x.Name.Equals(name));
			return findItem;
		}

		return null;
	}

	public void AddItem(IInventoryItem item)
	{
		if (items.Count < SLOT_SIZE)
		{
			// item.OnPickup();

			// 이미 가지고 있는 아이템인지 검사
			if (items.Exists(x => x.Name.Equals(item.Name)))
			{
				IInventoryItem findItem = items.Find(x => x.Name.Equals(item.Name));
				findItem.Count += item.Count;
				if ( ItemAdd != null )
					ItemAdd(this, new InventoryEventArgs(findItem));
				return;
			}

			if (ItemAdd != null)
			{
				ItemAdd(this, new InventoryEventArgs(item));
			}

			items.Add(item);
		}
	}

	public bool RemoveItem(InventoryItemData data)
	{
		if (data == null)
			return false;

		if (items.Exists(x => x.Name.Equals(data.name)))
		{
			IInventoryItem findItem = items.Find(x => x.Name.Equals(data.name));
			findItem.Count -= 1;
			if (ItemRemove != null)
				ItemRemove(this, new InventoryEventArgs(findItem));

			if (findItem.Count <= 0)
			{
				items.Remove(findItem);
			}
			return true;
		}

		return false;
	}

	public bool RemoveItem(IInventoryItem item)
	{
		if (items.Exists(x => x.Name.Equals(item.Name)))
		{
			IInventoryItem findItem = items.Find(x => x.Name.Equals(item.Name));
			findItem.Count -= 1;

			if (ItemRemove != null)
				ItemRemove(this, new InventoryEventArgs(findItem));

			if (findItem.Count <= 0)
			{
				items.Remove(findItem);

				Debug.Log(items.Exists(x => x.Name.Equals(item.Name)));
			}
			return true;
		}

		else
		{
			Debug.Log(item.Name + "이 없습니다.");
		}

		return false;
	}

	public InventoryItemData GetItem(int slotId)
	{
		switch (slotId)
		{
			case 1:
				return FindItemData(EItemType.HealPotion.ToString());
			case 2:
				return FindItemData(EItemType.ManaPotion.ToString());
		}
		return null;
	}

	public InventoryItemData FindItemData(string name)
	{
		if (!items.Exists(x => x.Name.Equals(name)))
			return null;
		IInventoryItem findItem = items.Find(x => x.Name.Equals(name));

		InventoryItemData param = new InventoryItemData(findItem.Name, findItem.Count, findItem.Param);

		return param;
	}
}
