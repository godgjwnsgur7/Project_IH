using UnityEngine;
using UnityEngine.UI;

public class TestSceneMainMenuView : View
{
	[SerializeField] private Button settingButton;
	[SerializeField] private Button exitButton;
	public override void Initialize()
	{
 		settingButton.onClick.AddListener(() => ViewManager.Show<TestSceneSettingMenuView>());
		exitButton.onClick.AddListener(() => ViewManager.ShowLast());
	}
}
