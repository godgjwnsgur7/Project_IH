using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BaseWindow : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false);
        return true;
    }

    public virtual void OpenWindowUI()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void SetInfo(UIParam param) { }

    public virtual void CloseWindowUI()
    {
        Managers.UI.CloseWindowUI(this);
    }
}
