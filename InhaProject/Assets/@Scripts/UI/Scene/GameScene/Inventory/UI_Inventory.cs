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
		IInventoryItem item = inventory.FindItem(e.Item.Param.type.ToString());
		Debug.Log(e.Item.Param.type.ToString());
		Debug.Log(item);
			
		foreach (UI_ItemSlot slot in itemSlots)
		{
			Transform childTransformFrontImg = slot.transform.Find("FrontImage");
			Image frontImage = childTransformFrontImg.GetComponent<Image>();

			if (item != null)
			{
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
		Debug.Log("아이템 사용 들어옴");

		foreach (UI_ItemSlot slot in arr)
		{
			Transform childTransformFrontImg = slot.transform.Find("FrontImage");
			Image frontImage = childTransformFrontImg.GetComponent<Image>();
			
			if (slot.name == item.Name)
			{
				int count = int.Parse(slot.countText.text);
				count -= 1;
				
				if ( slot.type == EItemSlotType.HealPotion ||
					slot.type == EItemSlotType.ManaPotion)
				{
					Debug.Log(slot.name + ", " + item.Name + " 같다.");
					Debug.Log(count);

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
