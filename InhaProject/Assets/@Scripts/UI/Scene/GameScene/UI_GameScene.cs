using System;
using System.Collections;
using System.Collections.Generic;
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
	private UI_SkillBar uiSkillBar;
	private UI_HealthBar uiHealthBar;

	public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Transform childTransformSkillBar;
        childTransformSkillBar = transform.Find("UI_SkillBar");

        if (childTransformSkillBar == null)
        {
            Debug.Log("UI_GameScene에 UI_SkillBar가 존재하지 않습니다.");
        }
        else
            uiSkillBar = childTransformSkillBar.GetComponent<UI_SkillBar>();

        Transform childTransformHealthBar;
        childTransformHealthBar = transform.Find("UI_HealthBar");

        if (childTransformHealthBar == null)
        {
            Debug.Log("UI_GameScene에 UI_HealthBar가 존재하지 않습니다.");
        }
        uiHealthBar = childTransformHealthBar.GetComponent<UI_HealthBar>();

        return true;
	}

    public void SetInfo(Action<EPlayerSkillType> OnReadyToSkill, float maxHp, float maxMp)
    {
		uiHealthBar.SetInfo(maxHp);
		uiSkillBar.SetInfo(OnReadyToSkill, maxMp);
	}

    public void ConnectPlayerInfoUI(Player player, Action<int> onSkillActive)
	{
		
	}

	// 테스트 용도
	public void OnClickDialogueButton()
	{
		string[] scripts = { "잘 되는지 확인", "확인확인확인", "마지막 확인 " };
		UIParam dialogueParam = new UIDialogueParam("아이고", scripts, scripts.Length);

		Managers.UI.OpenPopupUI<UI_Dialogue>(dialogueParam);
	}
}
