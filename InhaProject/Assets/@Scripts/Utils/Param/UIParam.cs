using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class UIDamageParam : UIParam
{
    public int damage;
    public Vector3 enermyPosition;
    public bool isCritical;

    public UIDamageParam(int damage, Vector3 enermyPosition, bool isCritical)
	{
		this.damage = damage;
		this.enermyPosition = enermyPosition;
		this.isCritical = isCritical;
	}
}