using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseView : ViewManager
{
    public EView ViewType { get; protected set; } = EView.Unknown;

	protected bool _init = false;

	public virtual bool Init()
	{
		if (_init)
			return false;

		_init = true;
		return true;
	}

	public virtual void Hide() => gameObject.SetActive(false);
	public virtual void Show() => gameObject.SetActive(true);

	private void Awake()
	{
		Init();
	}
}
