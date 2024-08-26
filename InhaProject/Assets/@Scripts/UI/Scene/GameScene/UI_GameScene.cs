using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum EPlayerSkillType
{
	Default = 0,
	Jump,
	Attack,
	Skill1,
	Skill2,
	Skill3
}

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

public class PlayerSkill
{
	public EPlayerSkillType skill;
	public float coolTime = 0.0f;
	public float currentTime = 0.0f;
	public bool isAvailable = true;
	public float mpAmount = 0.0f;

	public PlayerSkill(EPlayerSkillType skill, float coolTime, bool isAvailable, float mpAmount)
	{
		this.skill = skill;
		this.coolTime = coolTime;
		this.isAvailable = isAvailable;
		this.mpAmount = mpAmount;
		
		this.currentTime = 0.0f;
	}
}

public class UI_GameScene : UI_BaseScene
{
	private UI_SkillBar uiSkillBar;
	private UI_HealthBar uiHealthBar;
	protected UIPlayerData playerData;

	public override bool Init()
    {
        if (base.Init() == false)
            return false;

		PlayerSkill[] skills = new PlayerSkill[6];
		skills[(int)EPlayerSkillType.Default] = new PlayerSkill(EPlayerSkillType.Default, 1.0f, true, 0.0f);
		skills[(int)EPlayerSkillType.Jump] = new PlayerSkill(EPlayerSkillType.Jump, 1.0f, true, 0.0f);
		skills[(int)EPlayerSkillType.Attack] = new PlayerSkill(EPlayerSkillType.Attack, 1.0f, true, 0.0f);
		skills[(int)EPlayerSkillType.Skill1] = new PlayerSkill(EPlayerSkillType.Skill1, 3.0f, true, 10.0f);
		skills[(int)EPlayerSkillType.Skill2] = new PlayerSkill(EPlayerSkillType.Skill2, 3.0f, true, 20.0f);
		skills[(int)EPlayerSkillType.Skill3] = new PlayerSkill(EPlayerSkillType.Skill3, 4.0f, true, 30.0f);

		playerData = new UIPlayerData(100, 100, 100, 100, skills);

		return true;
	}

	private void Start()
	{
		Transform childTransformSkillBar;
		childTransformSkillBar = transform.Find("UI_SkillBar");

		if ( childTransformSkillBar == null)
		{
			Debug.Log("UI_GameScene에 UI_SkillBar가 존재하지 않습니다.");
		}
		else
			uiSkillBar = childTransformSkillBar.GetComponent<UI_SkillBar>();

		Transform childTransformHealthBar;
		childTransformHealthBar = transform.Find("UI_HealthBar");

		if ( childTransformHealthBar == null )
		{
			Debug.Log("UI_GameScene에 UI_HealthBar가 존재하지 않습니다.");
		}
		uiHealthBar = childTransformHealthBar.GetComponent<UI_HealthBar>();
	}

	public void SetPlayerDataInfo(UIPlayerData playerData)
	{
		this.playerData = playerData;
	}
}
