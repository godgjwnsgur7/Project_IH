using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TitleScene : UI_BaseScene
{
    [SerializeField] private TextMeshProUGUI blinkText;

    private Coroutine textEffectCoroutine = null;
    private bool textEffectLock = false;

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
        Debug.Log("Å¬¸¯");
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }
}
