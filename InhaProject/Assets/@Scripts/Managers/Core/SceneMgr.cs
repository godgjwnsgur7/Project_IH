using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr
{
    public BaseScene CurrentScene { get; private set; }

    private Define.EScene nextScene = Define.EScene.Unknown;
    private bool isCompleteLoadingScene = false;

    public void SetCurrentScene(BaseScene currScene)
    {
        CurrentScene = currScene;
    }

    public bool IsCompleteLoadingScene() => isCompleteLoadingScene;

    public void LoadScene(Define.EScene type)
    {
        nextScene = type;
        isCompleteLoadingScene = false;

        UIFadeEffectParam param = new UIFadeEffectParam(IsCompleteLoadingScene, LoadSceneAsync);

        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    public void LoadSceneAsync()
    {
        Managers.Clear();
        CoroutineHelper.StartCoroutine(ILoadSceneAsync(nextScene));
    }

    private IEnumerator ILoadSceneAsync(Define.EScene type)
    {
        AsyncOperation AsyncLoad = SceneManager.LoadSceneAsync(GetSceneName(type));
        
        yield return new WaitUntil(() => AsyncLoad.isDone);

        if (type == Define.EScene.GameScene)
            Managers.Game.Init();

        isCompleteLoadingScene = true;
    }

    private string GetSceneName(Define.EScene type)
    {
        string name = System.Enum.GetName(typeof(Define.EScene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
