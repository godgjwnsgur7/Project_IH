using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ClearPopup : UI_BasePopup
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
}