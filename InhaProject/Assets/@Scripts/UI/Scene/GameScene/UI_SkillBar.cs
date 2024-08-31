using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_SkillBar : UI_GameScene
{
	[SerializeField] private UI_Slot[] skillSlots;
	[SerializeField] private Image playerMPImg;
	[SerializeField] private Image playerMPImgBar;
	[SerializeField] private Text mpText;

	float time;

	public GameObject hudDamageText;

	// 테스트용도, 플레이어 MP 따로 받아와야 함
	public float playerMp = 100.0f;
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	private void Start()
	{
		for ( int i = 0; i < skillSlots.Length; i++ )
		{
			skillSlots[i].frontImage.fillAmount = 0;
			skillSlots[i].frontImage.fillOrigin = (int)Image.Origin360.Top;
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
		if (playerMp < playerData.skillDatas[(int)type].mpAmount)	
			yield break;

		playerMp -= playerData.skillDatas[(int)type].mpAmount;
		SetMpSlot();
		/*----------------------------------------------*/

		playerData.skillDatas[(int)type].isAvailable = false;

		while (playerData.skillDatas[(int)type].currentTime < playerData.skillDatas[(int)type].coolTime)
		{
			playerData.skillDatas[(int)type].currentTime += Time.deltaTime;
			skillSlots[(int)type].frontImage.fillAmount = playerData.skillDatas[(int)type].currentTime / playerData.skillDatas[(int)type].coolTime;
			yield return new WaitForFixedUpdate();
		}

		playerData.skillDatas[(int)type].currentTime = 0;
		playerData.skillDatas[(int)type].isAvailable = true;
		skillSlots[(int)type].frontImage.fillAmount = 0;
	}

	private void SetMpSlot()
	{
		playerMPImg.fillAmount = playerMp / 100.0f;
		playerMPImgBar.fillAmount = playerMp / 100.0f;
		mpText.text = playerMp + " / 100";
	}

	public void ConnectSlot()
	{

	}

    #region Skill Events
    public void OnClickSlot1()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Default].isAvailable ) 
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Default));
	}

	public void OnClickSlot2()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Guard].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Guard));

		UIInformParam uiInformParam = new UIInformParam("테스트테스트 테스트를 합시다");
		Managers.UI.OpenPopupUI<UI_InformPopup>(uiInformParam);
	}

	public void OnClickSlot3()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill1].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill1));

		UISelectParam uiSelectParam = new UISelectParam("테스트를 또 합니다. 예를 누르면 4번 슬롯이 눌립니다.", OnClickSlot4);
		Managers.UI.OpenPopupUI<UI_SelectPopup>(uiSelectParam);
	}

	public void OnClickSlot4()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill2].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill2));
	}

	public void OnClickSlot5()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill3].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill3));
	}

	public void OnClickSlot6()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill4].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill4));
	}
	#endregion

	public void OnDamageButton()
	{
        UIDamageParam uiDamageParam = new UIDamageParam(9900999, new Vector3(0, 0, 0), true);
        UI_Damage damageUI = Managers.Resource.Instantiate($"{PrefabPath.UI_OBJECT_PATH}/{EUIObjectType.UI_Damage}").GetComponent<UI_Damage>();
		
		if (damageUI != null)
			damageUI.SetInfo(uiDamageParam);
	}
}