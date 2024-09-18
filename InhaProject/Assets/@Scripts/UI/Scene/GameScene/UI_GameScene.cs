using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIPlayerData
{
	public float maxHp;
	public float currentHp;
	public float maxMp;
	public float currentMp;
	public PlayerSkill[] skillDatas = new PlayerSkill[6];

	public UIPlayerData(float maxHp, float currentHp, float maxMp, float currentMp, PlayerSkill[] skillDatas)
	{
		this.maxHp = maxHp;
		this.currentHp = currentHp;	
		this.maxMp = maxMp;	
		this.currentMp = currentMp;
		this.skillDatas = skillDatas;
	}
}

public class UI_GameScene : UI_BaseScene
{
    [SerializeField] private UI_HealthBar uiHealthBar;
    [SerializeField] private UI_SkillBar uiSkillBar;

    [SerializeField] public UI_Inventory uiInventory;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
	}

    public void SetInfo(Action<EPlayerSkillType> OnReadyToSkill, float maxHp, float maxMp)
    {
		uiHealthBar.SetInfo(maxHp);
		uiSkillBar.SetInfo(OnReadyToSkill, maxMp);
	}
	
	// 테스트 용도
	public void OnClickDialogueButton()
	{
		string[] scripts = { "잘 되는지 확인", "확인확인확인", "마지막 확인 " };
		UIParam dialogueParam = new UIDialogueParam("아이고", scripts);

		Managers.UI.OpenPopupUI<UI_Dialogue>(dialogueParam);
	}
}
