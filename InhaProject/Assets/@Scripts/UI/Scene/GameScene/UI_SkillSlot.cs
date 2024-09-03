using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillSlot : UI_Slot
{
    [SerializeField] TextMeshProUGUI cooltimeText;

    private void Awake()
    {
        Init();
    }

    override public void Init()
    {
        base.Init();

        if (front_img != null)
            frontImage.sprite = front_img;

        cooltimeText.enabled = false;
    }
}
