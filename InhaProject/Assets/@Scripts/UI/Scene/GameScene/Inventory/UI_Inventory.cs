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
	public Inventory inventory = new Inventory();

	[SerializeField, ReadOnly] UI_ItemSlot[] itemSlots;


	private void Start()
	{
		inventory = Managers.Scene.CurrentScene.GetComponent<GameScene>()?.inventory;

		if(inventory != null)
        {
			inventory.ItemAdd -= InventoryScript_ItemAdd;
			inventory.ItemAdd += InventoryScript_ItemAdd;
			inventory.ItemRemove -= InventoryScript_ItemRemove;
			inventory.ItemRemove += InventoryScript_ItemRemove;
		}
	}

	private void InventoryScript_ItemAdd(object sender, InventoryEventArgs e )
	{
		IInventoryItem item = inventory.FindItem(e.Item.Name);

		foreach (UI_ItemSlot slot in itemSlots)
		{
			Transform childTransformFrontImg = slot.transform.Find("FrontImage");
			Image frontImage = childTransformFrontImg.GetComponent<Image>();

			if (item != null)
			{
				if (slot.name == e.Item.Name)
				{
					slot.countText.text = e.Item.Count.ToString();
					break;
				}
				continue;
			}

			if (slot.name == e.Item.Name)
			{	
				frontImage.enabled = false;

				if (slot.type == EItemSlotType.HealPotion ||
					slot.type == EItemSlotType.ManaPotion)
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
			
			if (slot.name == item.Name)
			{
				
				if ( slot.type == EItemSlotType.HealPotion ||
					slot.type == EItemSlotType.ManaPotion)
				{
					if (e.Item.Count > 0)
					{
						slot.countText.text = e.Item.Count.ToString();
					}
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
