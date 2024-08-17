using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public enum EViewType
{
    Unknown,
    FixedView,
    MainView,
    SettingView,
    InventoryView
}

public class BaseView : ViewController
{
    public EViewType ViewType { get; protected set; } = EViewType.Unknown;

	Canvas canvas;

	protected bool _init = false;
	public int sorting = 10;
	public bool isActive = false;

	public virtual bool Init()
	{
		if (_init)
			return false;

		_init = true;
		return true;
	}

	public virtual void Hide() => gameObject.SetActive(false);
	public virtual void Show()
	{
		gameObject.SetActive(true);
		isActive = true;
	}

	private void Awake()
	{
		Init();
		canvas = gameObject.AddComponent<Canvas>();
		canvas.overrideSorting = true;
	}

	protected void SetSortingNum(int num)
	{
		sorting = num;
		canvas.sortingOrder = sorting;
	}
}
