using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : UI_BasePopup
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI scriptText;

	private string npcName;
	private string[] scripts;
	private int scriptLength;
	private int currentLine = 0;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	public override void OpenPopupUI()
	{
		base.OpenPopupUI();
	}

	public void SetCavnasSortingOrder(int sortingOrder)
    {
		Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);

		if ( canvas != null )
		{
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.overrideSorting = true;
			canvas.sortingOrder = sortingOrder;
		}
	}

	public override void SetInfo(UIParam param) 
	{
		UIDialogueParam test = param as UIDialogueParam;

		if (test == null)
			return;


		if (test is UIDialogueParam uiDialogueParam)
		{
			currentLine = 0;

			npcName = uiDialogueParam.nameText;
			scripts = uiDialogueParam.scriptTexts;
			scriptLength = uiDialogueParam.scriptTexts.Length;

			nameText.text = npcName;
			scriptText.text = scripts[currentLine];
		}
	}

	public override void ClosePopupUI()
	{
		base.ClosePopupUI();
	}

	public void OnClickNextButton()
	{
		if ( scriptLength - 1 <= currentLine )
		{
			ClosePopupUI();
			return;
		}

		currentLine++;
		scriptText.text = scripts[currentLine];
	}
}
