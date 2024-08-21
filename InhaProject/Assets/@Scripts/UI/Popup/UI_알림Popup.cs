using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// + 선택 팝업 -> 예 아니오를 받으셈
public class UI_알림Popup : UI_BasePopup
{
    [SerializeField] Text 알림메세지text;

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

        UI알림창Param test = param as UI알림창Param;
        if (test == null)
            return;

        // 동작

        if (param is UI알림창Param ui알림창Param)
        {
            알림메세지text.text = ui알림창Param.알림메세지;
        }
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        
        // 꺼진 후 동작
    }
}
