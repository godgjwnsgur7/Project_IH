using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager instance;

	[SerializeField] private View startingView;

	[SerializeField] private View fixedView;

	[SerializeField] private View[] views;

	private View currentView;

	private readonly Stack<View> history = new Stack<View>();

	public static T GetView<T>() where T : View
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
	
	public static void Show<T>(bool renderer = true) where T : View
	{
		for (int i = 0; i < instance.views.Length; i++)
		{
			if ( instance.views[i] is T)
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

	public static void Show(View view, bool renderer = true)
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

	public static void ShowFixedMenu(View view, bool renderer = true)
	{
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

	private void Awake() => instance = this;

	private void Start()
	{
		for ( int i = 0; i < views.Length; i++ )
		{
			views[i].Initialize();
			views[i].Hide();
		}

		if (fixedView != null)
		{
			Show(fixedView, true);
		}

		if (startingView != null)
		{
			Show(startingView, true);
		}
	}
}
