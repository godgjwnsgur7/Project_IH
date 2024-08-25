using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BasePopup : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false);

        return true;
    }

    public virtual void OpenPopupUI()
    {
        Managers.UI.SetCanvas(gameObject, true);
        this.gameObject.SetActive(true);
    }

    public virtual void SetInfo(UIParam param) { }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    /// <summary>
    /// 매니저에서만 호출해야하는 메서드
    /// </summary>
    public virtual void DeActivePopup()
    {
        this.gameObject.SetActive(false);
    }
}
