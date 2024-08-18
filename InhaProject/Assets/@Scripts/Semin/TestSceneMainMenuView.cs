using UnityEngine;
using UnityEngine.UI;

public class TestSceneMainMenuView : BaseView
{
	[SerializeField] private Button settingButton;
	[SerializeField] private Button exitButton;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ViewType = EViewType.MainView;

        settingButton.onClick.AddListener(() => ViewController.Show<TestSceneSettingMenuView>());
        exitButton.onClick.AddListener(() => ViewController.Hide<TestSceneMainMenuView>()); 
        return true;
    }
}