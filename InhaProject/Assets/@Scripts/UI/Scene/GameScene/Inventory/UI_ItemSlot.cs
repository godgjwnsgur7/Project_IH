using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Data;

public class UI_ItemSlot : UI_Slot
{
	[SerializeField, ReadOnly] public TextMeshProUGUI countText;
	[SerializeField, ReadOnly] public EItemType type;
	private bool isEnable { get; set; }

	private void Awake()
	{
		Init();
	}

	override public void Init()
	{
		base.Init();

		if (front_img != null)
			frontImage.sprite = front_img;

		countText.enabled = false;
		isEnable = false;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);

		foreach (EItemType item in Enum.GetValues(typeof(EItemType)))
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
