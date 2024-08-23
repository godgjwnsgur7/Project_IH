using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// + 선택 팝업 -> 예 아니오를 받으셈
public class UI_InformPopup : UI_BasePopup
{
    [SerializeField] Text informText;
    [SerializeField] Text acceptButtonText;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void OpenPopupUI()
    {
        base.OpenPopupUI();

        // 여기다 하는 게 맞음ㅋ
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        UIInformParam test = param as UIInformParam;
        if (test == null)
            return;

        // 동작

        if (param is UIInformParam uiInformParam)
        {
            informText.text = uiInformParam.informText;
		}
	}

	public override void ClosePopupUI()
	{
		base.ClosePopupUI();
		// 꺼진 후 동작
	}

    public void OnClickedAccepteButton()
	{
        ClosePopupUI();
	}
}
