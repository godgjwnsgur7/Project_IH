using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EPlayerSkillType
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
    public EPlayerSkillType skill;
    public float coolTime = 0.0f;
	public float currentTime = 0.0f;
	public bool isAvailable = true;
	public float mpAmount = 0.0f;

	public PlayerSkill(EPlayerSkillType skill, float coolTime, float currentTime, bool isAvailable, float mpAmount)
	{
		this.skill = skill;
		this.coolTime = coolTime;
		this.currentTime = currentTime;
		this.isAvailable = isAvailable;
		this.mpAmount = mpAmount;
	}
}


public class UI_SkillBar : MonoBehaviour
{
	PlayerSkill[] playerSkill = new PlayerSkill[6];
	[SerializeField] private Image[] coolTimeImgs;
	[SerializeField] private Image playerMPImg;
	[SerializeField] private Image playerMPImgBar;
	[SerializeField] private Text mpText;

	float time;

	// 테스트용도, 플레이어 MP 따로 받아와야 함
	public float playerMp = 100.0f;

	private void Start()
	{
		// 일단 플레이어 스킬 정보 박아넣기... 아마도 이런 정보를 어디선가 받아올테고...
        playerSkill[(int)EPlayerSkillType.Default] = new PlayerSkill(EPlayerSkillType.Default, 1.0f, 0.0f, true, 0.0f);
		playerSkill[(int)EPlayerSkillType.Jump] = new PlayerSkill(EPlayerSkillType.Jump, 1.0f, 0.0f, true, 0.0f);
		playerSkill[(int)EPlayerSkillType.Attack] = new PlayerSkill(EPlayerSkillType.Attack, 1.0f, 0.0f, true, 0.0f);
		playerSkill[(int)EPlayerSkillType.Skill1] = new PlayerSkill(EPlayerSkillType.Skill1, 3.0f, 0.0f, true, 10.0f);
		playerSkill[(int)EPlayerSkillType.Skill2] = new PlayerSkill(EPlayerSkillType.Skill2, 3.0f, 0.0f, true, 20.0f);
		playerSkill[(int)EPlayerSkillType.Skill3] = new PlayerSkill(EPlayerSkillType.Skill3, 4.0f, 0.0f, true, 30.0f);

		for ( int i = 0; i < coolTimeImgs.Length; i++ )
		{
			coolTimeImgs[i].fillAmount = 0;
			coolTimeImgs[i].fillOrigin = (int)Image.Origin360.Top;
		}

		/* TO DO: 플레이어 MP 따로 받아와서 처리해야 함 */
		SetMpSlot();
		/*----------------------------------------------*/
	}

	private void Update()
	{
		if ( Input.GetKeyDown(KeyCode.T))
		{
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Default));
		}
	}


	IEnumerator SkillCoolTime(EPlayerSkillType type)
	{
		/* TO DO: 플레이어 MP 따로 받아와서 처리해야 함 */
		if (playerMp < playerSkill[(int)type].mpAmount)	
			yield break;

		playerMp -= playerSkill[(int)type].mpAmount;
		SetMpSlot();
		/*----------------------------------------------*/

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

	private void SetMpSlot()
	{
		playerMPImg.fillAmount = playerMp / 100.0f;
		playerMPImgBar.fillAmount = playerMp / 100.0f;
		mpText.text = playerMp + " / 100";
	}

	#region Skill Events
	public void OnClickSlot1()
	{
		if (playerSkill[(int)EPlayerSkillType.Default].isAvailable ) 
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Default));
	}

	public void OnClickSlot2()
	{
		if (playerSkill[(int)EPlayerSkillType.Jump].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Jump));

		UIInformParam uiInformParam = new UIInformParam("테스트테스트 테스트를 합시다");
		Managers.UI.OpenPopupUI<UI_InformPopup>(uiInformParam);
	}

	public void OnClickSlot3()
	{
		if (playerSkill[(int)EPlayerSkillType.Attack].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Attack));
	}

	public void OnClickSlot4()
	{
		if (playerSkill[(int)EPlayerSkillType.Skill1].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill1));
	}

	public void OnClickSlot5()
	{
		if (playerSkill[(int)EPlayerSkillType.Skill2].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill2));
	}

	public void OnClickSlot6()
	{
		if (playerSkill[(int)EPlayerSkillType.Skill3].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill3));
	}
	#endregion

	public void OnDamageButton()
	{
		UIDamageParam uiDamageParam = new UIDamageParam(999999, new Vector3(400, 300, 0), true);
		Managers.UI.OpenPopupUI<UI_Damage>(uiDamageParam);
	}
}