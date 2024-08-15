using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class BaseView : InitBase
{
    public EView ViewType { get; protected set; } = EView.Unknown;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.View.SetCurrentScene(this);

        return true;
    }

    public abstract void Initialize();
	public virtual void Hide() => gameObject.SetActive(false);
	public virtual void Show() => gameObject.SetActive(true);
}
