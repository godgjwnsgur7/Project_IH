using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_SkillBar : UI_GameScene
{
	[SerializeField] private UI_SkillSlot[] skillSlots;
	[SerializeField] private Image playerMPImg;
	[SerializeField] private Image playerMPImgBar;
	[SerializeField] private Text mpText;

	float time;

	public GameObject hudDamageText;

	private Player player;

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
		for (int i = 0; i < skillSlots.Length; i++)
		{
			skillSlots[i].Init();
			skillSlots[i].frontImage.fillAmount = 0;
			skillSlots[i].frontImage.fillOrigin = (int)Image.Origin360.Top;
		}

		GameObject playerObject = GameObject.FindWithTag("Player");
		player = playerObject.GetComponent<Player>();

		if (player == null)
			return;

		player.OnChangedMp += OnChangedMp;
		player.OnUseSkill += OnUseSkill;

		OnChangedMp(player.PlayerInfo.CurrMp);
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

	private void Update()
	{

	}


	IEnumerator SkillCoolTime(int type)
	{
		Debug.Log(player.PlayerInfo.PlayerSkillList[type].isAvailable);

		if (player.PlayerInfo.PlayerSkillList[type].isAvailable == false)
			yield break;

		Debug.Log(player.PlayerInfo.PlayerSkillList[type].mpAmount);

		if (player.PlayerInfo.CurrMp < player.PlayerInfo.PlayerSkillList[type].mpAmount)	
			yield break;

		// player.PlayerInfo.CurrMp -= player.PlayerInfo.PlayerSkillList[type].mpAmount;

		// 이건 플레이어에서 해야 될 것 같은데
		player.PlayerInfo.PlayerSkillList[type].isAvailable = false;

		float currentTime = 0;   

		while (currentTime < player.PlayerInfo.PlayerSkillList[type].coolTime)
		{
			currentTime += Time.deltaTime;

			skillSlots[type].frontImage.fillAmount = currentTime;
			yield return new WaitForFixedUpdate();
		}

		skillSlots[(int)type].frontImage.fillAmount = 0;

		// 이건 플레이어에서 해야 될 것 같은데
		player.PlayerInfo.PlayerSkillList[type].isAvailable = true;
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
}