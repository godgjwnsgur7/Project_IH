using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI_TitleScene : SettingMenu
{
    private void Start()
    {
    }


    public void OnClickStart()
    {
        Debug.Log("����");
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }

    public void OnClickClose()
    {
        Debug.Log("����");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }


}


