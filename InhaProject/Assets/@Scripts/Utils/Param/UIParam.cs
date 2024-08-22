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

public class UI알림창Param : UIParam
{
    public string 알림메세지;

    public UI알림창Param(string 알림메세지)
    {
        this.알림메세지 = 알림메세지;
    }
}