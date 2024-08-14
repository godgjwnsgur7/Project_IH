using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneFixedMenuView : View
{
	[SerializeField] private Button exButton;
	public override void Initialize()
	{
		exButton.onClick.AddListener(() => ViewManager.Show<TestSceneMainMenuView>());
	}
}
