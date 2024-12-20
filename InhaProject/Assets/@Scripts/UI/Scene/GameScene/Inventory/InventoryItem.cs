using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
	string Name { get; }
	Sprite Image { get; }
	int Count { get; set; }
	ItemParam Param { get; set;  }
	void OnPickup();
}

public class InventoryEventArgs : EventArgs
{
	public IInventoryItem Item;
	public InventoryEventArgs(IInventoryItem item)
	{
		Item = item;
	}
}
