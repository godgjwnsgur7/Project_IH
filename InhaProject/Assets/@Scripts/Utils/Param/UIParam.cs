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

    public UISelectParam(string scriptText, UnityAction acceptAction, string acceptButtonText = "��", string declineButtonText = "�ƴϿ�")
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

public class UIDialogueParam : UIParam
{
    public string nameText;
    public string[] scriptTexts;
    public int size;

    public UIDialogueParam(string nameText, string[] scriptTexts, int size)
	{
		this.nameText = nameText;
		this.scriptTexts = scriptTexts;
		this.size = size;
	}   
}