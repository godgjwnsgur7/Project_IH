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

        settingButton.onClick.AddListener(() => ViewManager.Show<TestSceneSettingMenuView>());
        exitButton.onClick.AddListener(() => ViewManager.ShowLast());
        return true;
    }
}