using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_SkillBar : UI_GameScene
{
	[SerializeField] private Image[] coolTimeImgs;
	[SerializeField] private Image playerMPImg;
	[SerializeField] private Image playerMPImgBar;
	[SerializeField] private Text mpText;

	float time;

	public GameObject hudDamageText;

	// �׽�Ʈ�뵵, �÷��̾� MP ���� �޾ƿ;� ��
	public float playerMp = 100.0f;
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	private void Start()
	{
		for ( int i = 0; i < coolTimeImgs.Length; i++ )
		{
			coolTimeImgs[i].fillAmount = 0;
			coolTimeImgs[i].fillOrigin = (int)Image.Origin360.Top;
		}

		/* TO DO: �÷��̾� MP ���� �޾ƿͼ� ó���ؾ� �� */
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
		/* TO DO: �÷��̾� MP ���� �޾ƿͼ� ó���ؾ� �� */
		if (playerMp < playerData.skillDatas[(int)type].mpAmount)	
			yield break;

		playerMp -= playerData.skillDatas[(int)type].mpAmount;
		SetMpSlot();
		/*----------------------------------------------*/

		playerData.skillDatas[(int)type].isAvailable = false;

		while (playerData.skillDatas[(int)type].currentTime < playerData.skillDatas[(int)type].coolTime)
		{
			playerData.skillDatas[(int)type].currentTime += Time.deltaTime;
			coolTimeImgs[(int)type].fillAmount = playerData.skillDatas[(int)type].currentTime / playerData.skillDatas[(int)type].coolTime;
			yield return new WaitForFixedUpdate();
		}

		playerData.skillDatas[(int)type].currentTime = 0;
		playerData.skillDatas[(int)type].isAvailable = true;
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
		if (playerData.skillDatas[(int)EPlayerSkillType.Default].isAvailable ) 
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Default));
	}

	public void OnClickSlot2()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Jump].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Jump));

		UIInformParam uiInformParam = new UIInformParam("�׽�Ʈ�׽�Ʈ �׽�Ʈ�� �սô�");
		Managers.UI.OpenPopupUI<UI_InformPopup>(uiInformParam);
	}

	public void OnClickSlot3()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Attack].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Attack));

		UISelectParam uiSelectParam = new UISelectParam("�׽�Ʈ�� �� �մϴ�. ���� ������ 4�� ������ �����ϴ�.", OnClickSlot4);
		Managers.UI.OpenPopupUI<UI_SelectPopup>(uiSelectParam);
	}

	public void OnClickSlot4()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill1].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill1));
	}

	public void OnClickSlot5()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill2].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill2));
	}

	public void OnClickSlot6()
	{
		if (playerData.skillDatas[(int)EPlayerSkillType.Skill3].isAvailable)
			StartCoroutine(SkillCoolTime(EPlayerSkillType.Skill3));
	}
	#endregion

	public void OnDamageButton()
	{
		GameObject hudText = Instantiate(hudDamageText);
		UIDamageParam uiDamageParam = new UIDamageParam(9900999, new Vector3(0, 0, 0), true);
		hudText.GetComponent<UI_Damage>().SetInfo(uiDamageParam);
	}
}