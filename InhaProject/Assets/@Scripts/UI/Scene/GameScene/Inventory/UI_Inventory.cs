using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum EItemSlot
{
	RedPotion,
	BluePotion,
	IronKey,
	GoldenKey,
	Map
}

public class UI_Inventory : MonoBehaviour
{
	public GameObject item;
	public Inventory inventory = new Inventory();

	[SerializeField, ReadOnly] UI_ItemSlot[] itemSlots;


	public void OnClickPickupButton()
	{
		IInventoryItem inventoryItem = item.GetComponent<IInventoryItem>();

		if ( inventoryItem != null )
		{
			inventory.AddItem(inventoryItem);
		}
	}

	public void OnClickRemoveItemButton()
	{
		// InventoryItemData removeItemData = new InventoryItemData("HealPotion", 1, new ItemParam param);

		//if (removeItemData != null )
		//{
		//	inventory.RemoveItem(removeItemData);
		//}
	}

	private void Start()
	{
        inventory.ItemAdd -= InventoryScript_ItemAdd;
        inventory.ItemAdd += InventoryScript_ItemAdd;
        inventory.ItemRemove -= InventoryScript_ItemRemove;
        inventory.ItemRemove += InventoryScript_ItemRemove;

		HealPotion potion = new HealPotion();
		inventory.AddItem(potion);
	}

	private void InventoryScript_ItemAdd(object sender, InventoryEventArgs e )
	{
		IInventoryItem item = inventory.FindItem(e.Item.Name);

		foreach (UI_ItemSlot slot in itemSlots)
		{
			Transform childTransformFrontImg = slot.transform.Find("FrontImage");
			Image frontImage = childTransformFrontImg.GetComponent<Image>();

			Debug.Log(frontImage);
			Debug.Log(item);

			if (item != null)
			{
				Debug.Log(slot.name);
				if (slot.name == e.Item.Name)
				{
					int count = int.Parse(slot.countText.text);
					count += e.Item.Count;
					slot.countText.text = count.ToString();
					break;
				}
				continue;
			}

			if (slot.name == e.Item.Name)
			{
				frontImage.enabled = false;
				Debug.Log(slot.type);


				if (slot.type == EItemType.HealPotion ||
					slot.type == EItemType.ManaPotion)
				{
					slot.countText.text = e.Item.Count.ToString();
				}
				break;

			}
		}
	}

	private void InventoryScript_ItemRemove(object sender, InventoryEventArgs e)
	{
		UI_ItemSlot[] arr = transform.GetComponentsInChildren<UI_ItemSlot>();

		IInventoryItem item = inventory.FindItem(e.Item.Name);

		if ( item == null)
			return;

		foreach (UI_ItemSlot slot in arr)
		{
			Transform childTransformFrontImg = slot.transform.Find("FrontImage");
			Image frontImage = childTransformFrontImg.GetComponent<Image>();
			
			if (slot.name == e.Item.Name)
			{
				int count = int.Parse(slot.countText.text);
				//count = e.Item.Count;
				
				if ( e.Item.Param.type == EItemType.HealPotion ||
					slot.type == EItemType.ManaPotion)
				{
					if (count > 0)
						slot.countText.text = count.ToString();
					else
					{
						frontImage.enabled = true;
						slot.countText.text = "";
					}
				}
				
				else
				{
					frontImage.enabled = true;
				}
				break;
			}
		}
	}
}
