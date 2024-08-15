using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI_TitleScene : UI_BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }


    private void Start()
    {
    }


    public void OnClickStart()
    {
        Debug.Log("클릭");
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }

    public void OnClickClose()
    {
        Debug.Log("종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }


}
