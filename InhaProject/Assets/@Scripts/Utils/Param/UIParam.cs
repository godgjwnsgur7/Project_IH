using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIParam { }

public class UIFadeEffectParam : UIParam
{
    public Func<bool> fadeInEffectCondition;
    public Action onFadeOutCallBack;
    public Action onFadeInCallBack;

    public UIFadeEffectParam(Func<bool> fadeInEffectCondition = null, Action onFadeOutCallBack = null, Action onFadeInCallBack = null)
    {
        this.fadeInEffectCondition = fadeInEffectCondition;
        this.onFadeOutCallBack = onFadeOutCallBack;
        this.onFadeInCallBack = onFadeInCallBack;
    }
}

public class UIMonsterStatusParam : UIParam
{
    public BaseMonster monster;

    public UIMonsterStatusParam(BaseMonster monster)
    {
        this.monster = monster;
    }
}

public class UIBossMonsterStatusParam : UIParam
{
    public string monsterName;
    public float maxHp;

    public UIBossMonsterStatusParam(string monsterName, float maxHp)
    {
        this.monsterName = monsterName;
        this.maxHp = maxHp;
    }
}

public class UIInformParam : UIParam
{
    public string informText;

    public UIInformParam(string informText)
    {
        this.informText = informText;
    }
}

public class UISelectParam : UIParam
{
    public string scriptText;
    public string acceptButtonText;
    public string declineButtonText;
    public UnityAction acceptAction;

    public UISelectParam(string scriptText, UnityAction acceptAction, string acceptButtonText = "예", string declineButtonText = "아니오")
	{
		this.scriptText = scriptText;
		this.acceptButtonText = acceptButtonText;
		this.declineButtonText = declineButtonText;
        this.acceptAction = acceptAction;
	}
}

public class UIDamageParam : UIParam
{
    public int damage;
    public Vector3 pos;
    public bool isCritical;

    public UIDamageParam(int damage, Vector3 pos, bool isCritical = false)
	{
		this.damage = damage;
		this.pos = pos;
		this.isCritical = isCritical;
	}
}

public class UIHealParam : UIParam
{
    public int healAmount;
    public bool isHp;

    public UIHealParam(int healAmount, bool isHp)
    {
        this.healAmount = healAmount;
        this.isHp = isHp;
    }
}


public class UIDialogueParam : UIParam
{
    public string nameText;
    public string[] scriptTexts;

    public UIDialogueParam(string nameText, string[] scriptTexts)
	{
		this.nameText = nameText;
		this.scriptTexts = scriptTexts;
	}   
}

public class UITooltipParam : UIParam
{
    public string nameText;
    public string scriptText;

    public UITooltipParam(string nameText, string scriptText)
    {
        this.nameText = nameText;
        this.scriptText = scriptText;
    }
}

public class UITextParam : UIParam
{
    public string text;
    public float displayTime;

    public UITextParam(string text, float displayTime = 1f)
    {
        this.text = text;
        this.displayTime = displayTime;
    }
}