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

	public Canvas canvas;
    public GraphicRaycaster raycaster;

	protected bool _init = false;
	public bool isActive = false;

	public virtual bool Init()
	{
		if (_init)
			return false;

        _init = true;
		return true;
	}

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
	public virtual void Show()
	{
		gameObject.SetActive(true);
		isActive = true;
	}

	private void Awake()
    {
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 10;
            gameObject.SetActive(false);

            raycaster = gameObject.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects= GraphicRaycaster.BlockingObjects.None;
        }
        Init();
    }
}
