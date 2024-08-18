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

        canvas.sortingOrder = 0;
        exButton.onClick.AddListener(() => {
            ViewController.Show<TestSceneMainMenuView>(true);
        });

        return true;
    }
}