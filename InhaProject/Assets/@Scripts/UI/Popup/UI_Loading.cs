using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Loading : UI_BasePopup
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
}