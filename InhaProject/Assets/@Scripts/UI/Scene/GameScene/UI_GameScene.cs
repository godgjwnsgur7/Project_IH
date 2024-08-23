using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_BaseScene
{
    [SerializeField] UI_SkillBar uiSkillBar;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
}
