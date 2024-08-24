using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

	int order = 10;


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
	
	public static void Show<T>(bool sorting = true, bool renderer = true) where T : BaseView
	{
		for (int i = 0; i < instance.views.Length; i++)
		{
			if (instance.views[i] is T)
            {
				if (instance.views[i].isActive == false)
				{
					if (instance.currentView != null)
					{
						if (renderer)
						{
							instance.history.Push(instance.currentView);
						}

						instance.currentView.Hide();
					}

					if (sorting)
					{
                        instance.views[i].canvas.sortingOrder = instance.order;
						instance.order++;
					}
					else
						instance.views[i].canvas.sortingOrder = 0;

					instance.views[i].Show();
					instance.currentView = instance.views[i];

					instance.views[i].isActive = true;
					Debug.Log("order: " + instance.order + ", " + instance.currentView);
				}
			}
		}
	}

	public static void Show(BaseView view, bool sorting = true, bool renderer = true)
	{
		if (instance.currentView != null)
		{
			if (renderer)
			{
				instance.history.Push(instance.currentView);
			}

			instance.currentView.Hide();
		}

		if (view.isActive == false)
		{
			view.Show();
			view.isActive = true;
		}

        if (sorting)
        {
            view.canvas.sortingOrder = instance.order;
            instance.order++;
        }
        else
            view.canvas.sortingOrder = 0;

        instance.currentView = view;
	}

    public static void Hide<T>(bool sorting = true, bool renderer = true) where T : BaseView
	{
		if (instance.currentView == null)
			return;

		instance.currentView.Hide();
		instance.currentView.isActive = false;
		instance.currentView = null;

        if ( instance.history.Count != 0 )
		{
			instance.currentView = instance.history.Pop();
			instance.currentView.Show();
            Debug.Log("#Hide, order: " + instance.order);
        }

        instance.order--;
    }

    public static void ShowLast()
	{
		if (instance.history.Count != 0)
		{
			Show(instance.history.Pop(), false);
			instance.currentView.Hide();
		}
	}

	private void Awake()
	{
		instance = this;
		SetCanvas();
	}

	private void Start()
	{
		for (int i = 0; i < views.Length; i++)
		{
			if (views[i].canvas != null )
			{
				if (views[i].canvas.sortingOrder == 0)
				{
					views[i].Show();
				}
			}
		}

		//if (startingView != null)
		//{
		//	Show(startingView, true);
		//}
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
