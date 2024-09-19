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
					int count;
					if (slot.countText.text != "")
						count = int.Parse(slot.countText.text);
					else count = 0;
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

		Debug.Log("아이템 사용");
		
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
					int count;
					if (slot.countText.text != "")
						count = int.Parse(slot.countText.text);
					else count = 0;

					count -= 1;

					if (count > 0)
					{
						Debug.Log("카운트 개수 > 0 " + count);
						slot.countText.text = count.ToString();
					}
					else
					{
						Debug.Log(count + " <- count 개수");
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
