using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UI_GameScene : UI_BaseScene
{
    [SerializeField] UI_SkillBar uiSkillBar;
	[SerializeField] UI_HPBar uiHealthBar;

	public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
	}
}
