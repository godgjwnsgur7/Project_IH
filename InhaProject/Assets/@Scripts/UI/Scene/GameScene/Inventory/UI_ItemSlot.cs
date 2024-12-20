using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Data;

public enum EItemSlotType
{
	HealPotion,
	ManaPotion,
	Key,
	Map,
	Max
}

public class UI_ItemSlot : UI_Slot
{
	[SerializeField, ReadOnly] public TextMeshProUGUI countText;
	[SerializeField] public EItemSlotType type; 

	private void Awake()
	{
		Init();
	}

	override public void Init()
	{
		base.Init();

		if (front_img != null)
			frontImage.sprite = front_img;
			
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);

		foreach (EItemSlotType item in Enum.GetValues(typeof(EItemSlotType)))
		{
			if (item.ToString() == type.ToString())
			{
				JItemSlotData data = Managers.Data.ItemSlotDataDict[item.ToString()];
				UITooltipParam uiTooltipParam = new UITooltipParam(data.Name, data.Script);
				uiToolTip.SetInfo(uiTooltipParam);
			}
		}
	}
}
