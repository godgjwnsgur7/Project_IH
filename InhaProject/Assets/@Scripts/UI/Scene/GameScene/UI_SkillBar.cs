using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


public class UI_SkillBar : UI_BaseObject
{
	[SerializeField] private UI_SkillSlot[] skillSlots;
	[SerializeField] private Image playerMPImg;
	[SerializeField] private Image playerMPImgBar;
	[SerializeField] private Text mpText;

	float time;

	public GameObject hudDamageText;
    public event Action<int> OnReadyToSkill = null;

	// 지워야 하는 대 ㅋ 상 ㅋ
    private Player player;

    // 테스트용도, 플레이어 MP 따로 받아와야 함
    public float playerMp = 100.0f;
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].Init();
            skillSlots[i].frontImage.fillAmount = 0;
            skillSlots[i].frontImage.fillOrigin = (int)Image.Origin360.Top;
        }

		return true;
    }

    public void SetInfo(Action<int> OnReadyToSkill, float maxMp)
    {
        this.OnReadyToSkill = OnReadyToSkill;

        Managers.Game.Player.OnChangedMp -= OnChangedMp;
        Managers.Game.Player.OnChangedMp += OnChangedMp;

        Managers.Game.Player.OnUseSkill -= OnUseSkill;
        Managers.Game.Player.OnUseSkill += OnUseSkill;
    }

    private void OnChangedMp(float mp)
	{
		playerMPImg.fillAmount = mp / player.PlayerInfo.MaxMp;
		playerMPImgBar.fillAmount = mp / player.PlayerInfo.MaxMp;
		mpText.text = mp + " / " + player.PlayerInfo.MaxMp.ToString();
	}

	private void OnUseSkill(int type)
    {
		StartCoroutine(SkillCoolTime(type));
    }

	IEnumerator SkillCoolTime(int type)
	{
		Debug.Log(player.PlayerSkillDict[(EPlayerSkillType)type].isAvailable);

		if (player.PlayerSkillDict[(EPlayerSkillType)type].isAvailable == false)
			yield break;

		Debug.Log(player.PlayerSkillDict[(EPlayerSkillType)type].mpAmount);

		if (player.PlayerInfo.CurrMp < player.PlayerSkillDict[(EPlayerSkillType)type].mpAmount)	
			yield break;

		// player.PlayerInfo.CurrMp -= player.PlayerInfo.PlayerSkillList[type].mpAmount;

		// 이건 플레이어에서 해야 될 것 같은데
		player.PlayerSkillDict[(EPlayerSkillType)type].isAvailable = false;

		float currentTime = 0;   

		while (currentTime < player.PlayerSkillDict[(EPlayerSkillType)type].coolTime)
		{
			currentTime += Time.deltaTime;

			skillSlots[type].frontImage.fillAmount = currentTime;
			int cooltime = (int)(player.PlayerSkillDict[(EPlayerSkillType)type].coolTime - currentTime);
			skillSlots[(int)type].cooltimeText.text = cooltime.ToString();
			yield return new WaitForFixedUpdate();
		}

		skillSlots[(int)type].frontImage.fillAmount = 0;
		skillSlots[(int)type].cooltimeText.enabled = false;

		// 이건 플레이어에서 해야 될 것 같은데
		player.PlayerSkillDict[(EPlayerSkillType)type].isAvailable = true;
	}

	public void ConnectSlot()
	{

	}

	#region Skill Events
	//   public void OnClickSlot1()
	//{
	//	if (playerData.skillDatas[(int)EPlayerSkillType.Default].isAvailable ) 
	//		StartCoroutine(SkillCoolTime(EPlayerSkillType.Default));
	//}

	//public void OnClickSlot2()
	//{
	//	if (playerData.skillDatas[(int)EPlayerSkillType.Guard].isAvailable)
	//		StartCoroutine(SkillCoolTime(EPlayerSkillType.Guard));

	//	UIInformParam uiInformParam = new UIInformParam("테스트테스트 테스트를 합시다");
	//	Managers.UI.OpenPopupUI<UI_InformPopup>(uiInformParam);
	//}

	//public void OnClickSlot3()
	//{
	//	if (playerData.skillDatas[(int)EPlayerSkillType.Skill1].isAvailable)
	//		StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill1));

	//	UISelectParam uiSelectParam = new UISelectParam("테스트를 또 합니다. 예를 누르면 4번 슬롯이 눌립니다.", OnClickSlot4);
	//	Managers.UI.OpenPopupUI<UI_SelectPopup>(uiSelectParam);
	//}

	//public void OnClickSlot4()
	//{
	//	if (playerData.skillDatas[(int)EPlayerSkillType.Skill2].isAvailable)
	//		StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill2));
	//}

	//public void OnClickSlot5()
	//{
	//	if (playerData.skillDatas[(int)EPlayerSkillType.Skill3].isAvailable)
	//		StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill3));
	//}

	//public void OnClickSlot6()
	//{
	//	if (playerData.skillDatas[(int)EPlayerSkillType.Skill4].isAvailable)
	//		StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill4));
	//}
	#endregion

	public void OnDamageButton()
	{
		int rand = UnityEngine.Random.Range(0, 2);

		UIDamageParam uiDamageParam = new UIDamageParam(9900999, new Vector3(0, 0, 0), false);
		UI_Damage damageUI = Managers.Resource.Instantiate($"{PrefabPath.UI_OBJECT_PATH}/{EUIObjectType.UI_Damage}").GetComponent<UI_Damage>();

		switch ( rand )
        {
			default:
				uiDamageParam.isCritical = true;
				break;
			case 1:
				uiDamageParam.isCritical = false;
				break;
        }

		if (damageUI != null)
			damageUI.SetInfo(uiDamageParam);
	}
}