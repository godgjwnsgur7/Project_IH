using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	private const int SLOT_SIZE = 6;
	private List<IInventoryItem> items = new List<IInventoryItem>();

	public event EventHandler<InventoryEventArgs> ItemAdd;

	public void AddItem(IInventoryItem item)
	{
		if (items.Count < SLOT_SIZE)
		{
			// as BaseObjectÀÎÁö
			Collider collider = (item as BaseItem).GetComponent<Collider>();

			if (collider != null)
			{
				if (collider.enabled)
				{
					collider.enabled = false;
				}
			}

			item.OnPickup();

			if (ItemAdd != null)
			{
				ItemAdd(this, new InventoryEventArgs(item));
			}

			items.Add(item);
		}
	}
}
