using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        UIInformParam test = param as UIInformParam;
        if (test == null)
            return;


        if (param is UIInformParam uiInformParam)
        {
            informText.text = uiInformParam.informText;
		}
	}

	public override void ClosePopupUI()
	{
		base.ClosePopupUI();
	}

    public void OnClickAccepteButton()
	{
        ClosePopupUI();
	}
}
