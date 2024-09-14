using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Slot
{
	[SerializeField] public TextMeshProUGUI countText;
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

		//slotImage.enabled = false;
		countText.enabled = false;
		isEnable = false;

		base.SetInfo("������ ����", "������ ������ ���� ��");
	}
}
