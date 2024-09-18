using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.Progress;

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

public class Inventory
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
			Collider collider = (item as BaseItem).GetComponent<Collider>();

			if (collider != null)
			{
				if (collider.enabled)
				{
					collider.enabled = false;
				}
			}

			item.OnPickup();

			// 이미 가지고 있는 아이템인지 검사
			if (items.Exists(x => x.Name.Equals(item.Name)))
			{
				IInventoryItem findItem = items.Find(x => x.Name.Equals(item.Name));
				findItem.Count += item.Count;
				if ( ItemAdd != null )
					ItemAdd(this, new InventoryEventArgs(item));
				return;
			}

			if (ItemAdd != null)
			{
				ItemAdd(this, new InventoryEventArgs(item));
			}

			items.Add(item);
		}
	}

	public void RemoveItem(InventoryItemData data)
	{
		if (items.Exists(x => x.Name.Equals(data.name)))
		{
			IInventoryItem findItem = items.Find(x => x.Name.Equals(data.name));
			findItem.Count -= data.count;
			if (ItemRemove != null)
				ItemRemove(this, new InventoryEventArgs(findItem));

			if (findItem.Count <= 0)
			{
				items.Remove(findItem);
				Debug.Log(items.Exists(x => x.Name.Equals(data.name)));
			}
			return;
		}

		else
		{
			Debug.Log(data.name + "이 없습니다.");
		}
	}

	public void RemoveItem(IInventoryItem item)
	{
		if (items.Exists(x => x.Name.Equals(item.Name)))
		{
			IInventoryItem findItem = items.Find(x => x.Name.Equals(item.Name));
			findItem.Count -= item.Count;
			if (ItemRemove != null)
				ItemRemove(this, new InventoryEventArgs(findItem));

			if ( findItem.Count <= 0 )
			{
				items.Remove(findItem);
				Debug.Log(items.Exists(x => x.Name.Equals(item.Name)));
			}
			return;
		}

		else
		{
			Debug.Log(item.Name + "이 없습니다.");
		}
	}

	public InventoryItemData GetItem(int slotId)
	{
		return null;
	}
}
