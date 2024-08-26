using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectPopup : UI_BasePopup
{
    [SerializeField] Text ScriptText;
    [SerializeField] Text acceptButtonText;
    [SerializeField] Text declineButtonText;

    [SerializeField] Button acceptButton;
    [SerializeField] Button declineButton;

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

        UISelectParam test = param as UISelectParam;
        if (test == null)
            return;
        
        if (param is UISelectParam uiSelectParam)
        {
            ScriptText.text = uiSelectParam.scriptText;
            acceptButtonText.text = uiSelectParam.acceptButtonText;
            declineButtonText.text = uiSelectParam.declineButtonText;
            acceptButton.onClick.AddListener(uiSelectParam.acceptAction + ClosePopupUI);
        }
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    public void OnClickDeclineButton()
    {
        ClosePopupUI();
    }


}
