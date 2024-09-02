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
    }
}
