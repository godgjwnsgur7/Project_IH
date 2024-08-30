using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Slot
{
	[SerializeField] public TextMeshProUGUI countText;

	private void Start()
	{
		Init();
	}

	override protected void Init()
	{
		base.Init();
		slotImage.enabled = false;
		countText.enabled = false;
	}
}
