using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
			Debug.Log(inventoryItem.Name + " " + inventoryItem.Count);
			inventory.AddItem(inventoryItem);
			Destroy(item);
		}
	}

	private void Start()
	{
		inventory.ItemAdd += InventoryScript_ItemAdd;
		inventory.ItemRemove += InventoryScript_ItemRemove;
	}

	private void InventoryScript_ItemAdd(object sender, InventoryEventArgs e )
	{
		UI_ItemSlot[] arr = transform.GetComponentsInChildren<UI_ItemSlot>();

		foreach (UI_ItemSlot slot in arr)
		{
			Transform childTransformSlotImg = slot.transform.Find("SlotImage");
			Image image = childTransformSlotImg.GetComponent<Image>();

			if ( slot.name == e.Item.Name )
			{
				int count = int.Parse(slot.countText.text);
				count += e.Item.Count;
				slot.countText.text = count.ToString();
				break;
			}

			if ( !image.enabled )
			{
				image.enabled = true;
				image.sprite = e.Item.Image;
				slot.countText.enabled = true;
				slot.countText.text = e.Item.Count.ToString();
				break;
			}
		}
	}

	private void InventoryScript_ItemRemove(object sender, InventoryEventArgs e)
	{

	}
}
