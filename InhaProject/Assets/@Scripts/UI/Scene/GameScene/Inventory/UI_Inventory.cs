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
			inventory.AddItem(inventoryItem);
		}
	}

	public void OnClickRemoveItemButton()
	{
		InventoryData removeItemData = new InventoryData("HealPotion", 1, EItemType.HealPotion);

		if (removeItemData != null )
		{
			inventory.RemoveItem(removeItemData);
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

			// �̹� ������ �ִ� �������̶�� ���ڸ� ó��
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

			// ������ �ִ� �������� �ƴ϶�� ���Կ� ó��
			if ( !image.enabled )
			{
				slot.name = e.Item.Name;
				image.enabled = true;
				image.sprite = e.Item.Image;

				// ������ Ÿ���� �Ҹ�ǰ�̶�� ���ڵ� ǥ��
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
		UI_ItemSlot[] arr = transform.GetComponentsInChildren<UI_ItemSlot>();

		IInventoryItem item = inventory.FindItem(e.Item.Name);

		// ã�� �������� �κ��丮�� ���ٸ� return
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
				
				// �Ҹ�ǰ�� ��츸 ���� ǥ�� ����
				if ( e.Item.Type == EItemType.HealPotion)
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
