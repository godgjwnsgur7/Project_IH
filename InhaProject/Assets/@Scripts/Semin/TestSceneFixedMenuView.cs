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

        ViewType = EViewType.FixedView;
        sorting = 0;

        exButton.onClick.AddListener(() => ViewController.Show<TestSceneMainMenuView>());
        return true;
    }
}