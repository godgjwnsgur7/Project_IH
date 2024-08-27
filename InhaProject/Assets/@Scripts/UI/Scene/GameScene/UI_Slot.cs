using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
	[SerializeField] public Sprite slot_img;
	public Sprite front_img;
	private Image slotImage;
	public Image frontImage;


	private void Start()
	{
		Transform childTransformSlotImg = transform.Find("SlotImage");
		slotImage = childTransformSlotImg.GetComponent<Image>();

		Transform childTransformFrontImg = transform.Find("FrontImage");
		frontImage = childTransformFrontImg.GetComponent<Image>();

		slotImage.sprite = slot_img;
		frontImage.sprite = front_img;
	}
}
