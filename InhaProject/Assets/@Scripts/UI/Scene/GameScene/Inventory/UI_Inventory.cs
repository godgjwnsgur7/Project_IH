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
		Player player = Managers.Game.Player;

        inventory.ItemAdd -= InventoryScript_ItemAdd;
        inventory.ItemAdd += InventoryScript_ItemAdd;
        inventory.ItemRemove -= InventoryScript_ItemRemove;
        inventory.ItemRemove += InventoryScript_ItemRemove;
    }

	private void InventoryScript_ItemAdd(object sender, InventoryEventArgs e )
	{
		// UI_ItemSlot[] arr = transform.GetComponentsInChildren<UI_ItemSlot>();

		IInventoryItem item = inventory.FindItem(e.Item.Name);

		//switch (item.Param.type)
		//{
		//	case EItemType.None:
		//		break;
		//	case EItemType.HealPotion:
		//		itemSlots[(int)EItemSlot.RedPotion].name = e.Item.Name;
		//		itemSlots[(int)EItemSlot.RedPotion].frontImage.enabled = false;
		//		itemSlots[(int)EItemSlot.RedPotion].countText.enabled = true;
		//		itemSlots[(int)EItemSlot.RedPotion].countText.text = e.Item.Count.ToString();
		//		break;
		//	case EItemType.ManaPotion:
		//		itemSlots[(int)EItemSlot.BluePotion].name = e.Item.Name;
		//		itemSlots[(int)EItemSlot.BluePotion].frontImage.enabled = false;
		//		itemSlots[(int)EItemSlot.BluePotion].countText.enabled = true;
		//		itemSlots[(int)EItemSlot.BluePotion].countText.text = e.Item.Count.ToString();
		//		break;
		//	case EItemType.Key:

		//		break;
		//}

		foreach (UI_ItemSlot slot in itemSlots)
		{


			Transform childTransformSlotImg = slot.transform.Find("SlotImage");
			Image image = childTransformSlotImg.GetComponent<Image>();

			// 이미 가지고 있는 아이템이라면 숫자만 처리
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

			// 가지고 있던 아이템이 아니라면 슬롯에 처리
			if ( !image.enabled )
			{
				slot.name = e.Item.Name;
				image.enabled = true;
				image.sprite = e.Item.Image;

				// 아이템 타입이 소모품이라면 숫자도 표시
				if ( e.Item.Param.type == EItemType.HealPotion)
				{
					slot.countText.enabled = true;
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

		// 찾는 아이템이 인벤토리에 없다면 return
		if ( item == null)
			return;

		foreach (UI_ItemSlot slot in arr)
		{
			Transform childTransformSlotImg = slot.transform.Find("SlotImage");
			Image image = childTransformSlotImg.GetComponent<Image>();
			
			if (slot.name == e.Item.Name)
			{
				int count = int.Parse(slot.countText.text);
				count = e.Item.Count;
				
				// 소모품일 경우만 개수 표시 수정
				if ( e.Item.Param.type == EItemType.HealPotion)
				{
					if (count > 0)
						slot.countText.text = count.ToString();
					else
					{
						image.enabled = false;
						slot.countText.enabled = false;
						slot.name = "null";
					}
				}
				
				else
				{
					image.enabled = false;
					slot.name = "null";
				}
				break;
			}
		}
	}
}
