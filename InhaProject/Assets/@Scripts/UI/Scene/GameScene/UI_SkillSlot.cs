using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillSlot : UI_Slot
{
    [SerializeField, ReadOnly] private TextMeshProUGUI cooltimeText;

    private void Awake()
    {
        Init();
    }

    override public void Init()
    {
        base.Init();

        if (front_img != null)
            frontImage.sprite = front_img;


        frontImage.fillOrigin = (int)Image.Origin360.Top;
        frontImage.fillAmount = 0.0f;

        cooltimeText.text = "";
    }

    public void SetFillAmountToFrontImage(float value)
    {
        frontImage.fillAmount = value;
    }

    public void SetCooltimeText(string value)
    {
        cooltimeText.text = value;
    }
}
