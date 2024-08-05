using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingPopup : UI_BasePopup
{

    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();


    }

    // 로딩 완료를 받아서 
}