using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESkill
{
	Default = 0,
    Jump,
    Attack,
    Skill1,
    Skill2,
    Skill3
}

public class PlayerSkill
{
    public ESkill skill;
    public float coolTime = 0.0f;
	public float currentTime = 0.0f;
	public bool isAvailable = true;

	public PlayerSkill(ESkill skill, float coolTime, float currentTime, bool isAvailable)
	{
		this.skill = skill;
		this.coolTime = coolTime;
		this.currentTime = currentTime;
		this.isAvailable = isAvailable;
	}
}


public class UI_SkillBar : MonoBehaviour
{
	PlayerSkill[] playerSkill = new PlayerSkill[6];
	[SerializeField] private Image[] coolTimeImgs;
	[SerializeField] private Image playerMPImg;

	float time;

	private void Start()
	{
		// 일단 플레이어 스킬 정보 박아넣기... 아마도 이런 정보를 어디선가 받아올테고...
        playerSkill[(int)ESkill.Default] = new PlayerSkill(ESkill.Default, 1.0f, 0.0f, true);
		playerSkill[(int)ESkill.Jump] = new PlayerSkill(ESkill.Jump, 1.0f, 0.0f, true);
		playerSkill[(int)ESkill.Attack] = new PlayerSkill(ESkill.Attack, 1.0f, 0.0f, true);
		playerSkill[(int)ESkill.Skill1] = new PlayerSkill(ESkill.Skill1, 3.0f, 0.0f, true);
		playerSkill[(int)ESkill.Skill2] = new PlayerSkill(ESkill.Skill2, 3.0f, 0.0f, true);
		playerSkill[(int)ESkill.Skill3] = new PlayerSkill(ESkill.Skill3, 4.0f, 0.0f, true);

		for ( int i = 0; i < coolTimeImgs.Length; i++ )
		{
			coolTimeImgs[i].fillAmount = 0;
			coolTimeImgs[i].fillOrigin = (int)Image.Origin360.Top;
		}
	}

	private void Update()
	{
		if ( Input.GetKeyDown(KeyCode.T))
		{
			StartCoroutine(SkillCoolTime(ESkill.Default));
		}
	}

	
	public void SetMpImage()
	{

	}


	IEnumerator SkillCoolTime(ESkill type)
	{
		playerSkill[(int)type].isAvailable = false;

		while (playerSkill[(int)type].currentTime < playerSkill[(int)type].coolTime)
		{
			playerSkill[(int)type].currentTime += Time.deltaTime;
			coolTimeImgs[(int)type].fillAmount = playerSkill[(int)type].currentTime / playerSkill[(int)type].coolTime;
			yield return new WaitForFixedUpdate();
		}

		playerSkill[(int)type].currentTime = 0;
		playerSkill[(int)type].isAvailable = true;
		coolTimeImgs[(int)type].fillAmount = 0;
	}

	public void OnClickedSlot1()
	{
		if (playerSkill[(int)ESkill.Default].isAvailable ) 
			StartCoroutine(SkillCoolTime(ESkill.Default));
	}

	public void OnClickedSlot2()
	{
		if (playerSkill[(int)ESkill.Jump].isAvailable)
			StartCoroutine(SkillCoolTime(ESkill.Jump));
	}

	public void OnClickedSlot3()
	{
		if (playerSkill[(int)ESkill.Attack].isAvailable)
			StartCoroutine(SkillCoolTime(ESkill.Attack));
	}

	public void OnClickedSlot4()
	{
		if (playerSkill[(int)ESkill.Skill1].isAvailable)
			StartCoroutine(SkillCoolTime(ESkill.Skill1));
	}

	public void OnClickedSlot5()
	{
		if (playerSkill[(int)ESkill.Skill2].isAvailable)
			StartCoroutine(SkillCoolTime(ESkill.Skill2));
	}

	public void OnClickedSlot6()
	{
		if (playerSkill[(int)ESkill.Skill3].isAvailable)
			StartCoroutine(SkillCoolTime(ESkill.Skill3));
	}
}