using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour 
{
    private static ViewController instance;

	[SerializeField] private BaseView startingView;

	[SerializeField] private BaseView fixedView;

	[SerializeField] private BaseView[] views;
	public BaseView currentView { get; private set; }

	private readonly Stack<BaseView> history = new Stack<BaseView>();
	//public BaseView CurrentView { get; private set; }


    public void SetCurrentView(BaseView currView)
    {
        currentView = currView;
    }

    public static T GetView<T>() where T : BaseView
	{
		for (int i = 0; i < instance.views.Length; i++)
		{
			if (instance.views[i] is T tView)
			{
				return tView;
			}
		}

		return null;
	}
	
	public static void Show<T>(bool renderer = true) where T : BaseView
	{
		for (int i = 0; i < instance.views.Length; i++)
		{
			if (instance.views[i] is T)
			{
				if (instance.currentView != null)
				{
					if ( renderer )
					{
						instance.history.Push(instance.currentView);
					}

					if (instance.currentView != instance.fixedView)
						instance.currentView.Hide();
				}

				instance.views[i].Show();

				instance.currentView = instance.views[i];
			}
		}
	}

	public static void Show(BaseView view, bool renderer = true)
	{
		if (instance.currentView != null)
		{
			if (renderer)
			{
				instance.history.Push(instance.currentView);
			}

			if ( instance.currentView != instance.fixedView)
				instance.currentView.Hide();
		}
		
		view.Show();

		instance.currentView = view;
	}

	public static void ShowLast()
	{
		if (instance.history.Count != 0)
		{
			Show(instance.history.Pop(), false);
		}
	}

	private void Awake()
	{
		instance = this;
		SetCanvas();
	}

	private void Start()
	{
		//for ( int i = 0; i < views.Length; i++ )
		//{
		//	//if (views[i].sorting != 0)
		//		//views[i].Hide();
		//}

		if (startingView != null)
		{
			Show(startingView, true);
		}
	}

	public bool SetCanvas()
	{
		Canvas canvas = GetComponent<Canvas>();
		CanvasScaler scaler = GetComponent<CanvasScaler>();

		if (canvas != null)
		{
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.referenceResolution = new Vector2(1980, 1020);
			scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

			return true;
		}
		return false;
	}
}