using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneSettingMenuView : BaseView
{
    [SerializeField] private Button backButton;

    public override bool Init()
    {
        if (_init)
            return false;

        backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _init = true;
        return true;
    }
}
