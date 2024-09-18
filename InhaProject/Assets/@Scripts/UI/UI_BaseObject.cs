using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;

public enum EUIObjectType
{
    UI_Damage,
    UI_MonsterStatus,
    UI_BossMonsterStatus,
    UI_Tooltip,
    UI_TextObject,
}

public class UI_BaseObject : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false);

        return true;
    }

    public virtual void SetInfo(UIParam param)
    {

    }
}
