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
	private Inventory inventory;

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
		GameObject playerObject = GameObject.FindWithTag("Player");
		Player player = playerObject.GetComponent<Player>();

		//inventory = player.inventory;
		inventory.ItemAdd += InventoryScript_ItemAdd;
		inventory.ItemRemove += InventoryScript_ItemRemove;
	}

	private void InventoryScript_ItemAdd(object sender, InventoryEventArgs e )
	{
		UI_ItemSlot[] arr = transform.GetComponentsInChildren<UI_ItemSlot>();

		IInventoryItem item = inventory.FindItem(e.Item.Name);

		foreach (UI_ItemSlot slot in arr)
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
				if ( e.Item.Type == EItemType.HealPotion)
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

	}
}
