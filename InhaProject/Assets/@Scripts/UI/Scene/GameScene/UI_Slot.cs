using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
	[SerializeField] protected Sprite slot_img;
	[SerializeField] protected Sprite front_img;

	protected Image slotImage;
	public Image frontImage;

	private void Start()
	{
		Init();
	}

	virtual public void Init()
	{
		Transform childTransformSlotImg = transform.Find("SlotImage");
		slotImage = childTransformSlotImg.GetComponent<Image>();

		Transform childTransformFrontImg = transform.Find("FrontImage");
		frontImage = childTransformFrontImg.GetComponent<Image>();

		if (front_img != null)
			frontImage.sprite = front_img;
	}
}
