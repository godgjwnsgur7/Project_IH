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
	[SerializeField] private Image mpSlider;
	[SerializeField] private Image mpSliderBar;
	[SerializeField] private Text mpText;

	[SerializeField, ReadOnly] float maxMp = 0;
	[SerializeField, ReadOnly] float curMp = 0;

	public GameObject hudDamageText;
    public event Action<EPlayerSkillType> OnReadyToSkill = null;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
    }

    public void SetInfo(Action<EPlayerSkillType> OnReadyToSkill, float maxMp)
    {
        this.OnReadyToSkill = OnReadyToSkill;

		Managers.Game.Player.OnChangedMp -= OnChangedMp;
		Managers.Game.Player.OnChangedMp += OnChangedMp;

		Managers.Game.Player.OnUseSkill -= OnUseSkill;
		Managers.Game.Player.OnUseSkill += OnUseSkill;

		this.maxMp = maxMp;
		curMp = maxMp;

		SetMpBar();
	}

    private void OnChangedMp(float curMp)
	{
		this.curMp = curMp;
		SetMpBar();
	}

	private void OnUseSkill(EPlayerSkillType type, float coolTime)
    {
        StartCoroutine(CoSkillCoolTime(type, coolTime));
    }

	IEnumerator CoSkillCoolTime(EPlayerSkillType type, float coolTime)
	{
		UI_SkillSlot slot = skillSlots[(int)type - 1];

		if (slot == null)
		{
			Debug.LogWarning($"{(int)type}번 slot이 없음");
            yield break;
        }

        float currentTime = 0;

        while (currentTime < coolTime)
        {
            currentTime += Time.deltaTime;
            int cooltime = (int)(coolTime - currentTime) + 1;

            slot.SetFillAmountToFrontImage(currentTime / coolTime);
            slot.SetCooltimeText(cooltime.ToString());
            yield return null;
        }

        slot.SetFillAmountToFrontImage(0);
        slot.SetCooltimeText("");
        OnReadyToSkill?.Invoke(type);
    }

	private void SetMpBar()
    {
		mpSlider.fillAmount = curMp / maxMp;
		mpSliderBar.fillAmount = curMp / maxMp;
		mpText.text = $"{curMp} / {maxMp}";
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