using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.Progress;

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
				Debug.Log("아이템 없을걸");
			}
			return;
		}

		else
		{
			Debug.Log(item.Name + "이 없습니다.");
		}
	}
}
