using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneFixedMenuView : BaseView
{
	[SerializeField] private Button exButton;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ViewType = Define.EView.FixedView;

        exButton.onClick.AddListener(() => ViewManager.Show<TestSceneMainMenuView>());
        return true;
    }

    public override void Initialize()
	{
	}
}
