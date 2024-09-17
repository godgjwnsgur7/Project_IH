using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Data;

public class UI_SkillSlot : UI_Slot
{
    [SerializeField, ReadOnly] private TextMeshProUGUI cooltimeTextui;
    [SerializeField, ReadOnly] public EPlayerSkillType skillType;
    

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

        cooltimeTextui.text = "";
    }

    public override void OnPointerEnter(PointerEventData eventData) 
    {
        base.OnPointerEnter(eventData);
        
        foreach (EPlayerSkillType item in Enum.GetValues(typeof(EPlayerSkillType)))
        {
            if ( item.ToString() == skillType.ToString() )
            {
                JSkillSlotData data = Managers.Data.SkillSlotDict[item.ToString()];
                UITooltipParam uiTooltipParam = new UITooltipParam(data.Name, data.Script);
                uiToolTip.SetInfo(uiTooltipParam);
                uiToolTip.SetCooltimeText(data.Cooltime);
                uiToolTip.SetMpAmountText(data.MpAmount);
            }
        }
    }

    public void SetFillAmountToFrontImage(float value)
    {
        frontImage.fillAmount = value;
    }

    public void SetCooltimeText(string value)
    {
        cooltimeTextui.text = value;
    }
}
