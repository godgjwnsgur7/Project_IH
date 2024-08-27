using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Inventory : MonoBehaviour
{
	public GameObject item;
	public Inventory inventory;

	[SerializeField]
	Transform inventorySlot;


	public void OnClickPickupButton()
	{
		IInventoryItem inventoryItem = item.GetComponent<IInventoryItem>();
		if ( inventoryItem != null )
		{
			inventory.AddItem(inventoryItem);
		}
	}

	private void Start()
	{
		inventory.ItemAdd += InventoryScript_ItemAdd;
	}

	private void InventoryScript_ItemAdd(object sender, InventoryEventArgs e )
	{
		UI_Slot[] arr = transform.GetComponentsInChildren<UI_Slot>();

		foreach (UI_Slot slot in arr)
		{
			Debug.Log(slot.name);
		}

	}
}
