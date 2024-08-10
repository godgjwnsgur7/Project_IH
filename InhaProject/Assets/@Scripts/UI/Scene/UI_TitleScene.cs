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
        textEffectCoroutine = StartCoroutine(IBlinkEffectToText(20));
    }

    public void OnClickStart()
    {
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }

    private IEnumerator IBlinkEffectToText(int fadePercent)
    {
        textEffectLock = true;
        Color color = new Color(1, 1, 1, 1);
        float runTIme;
        float duration = 1.0f;
        float fadeValue = fadePercent * 0.01f;

        while (textEffectLock)
        {
            runTIme = 0.0f;
            color.a = 1.0f;
            blinkText.color = color;
            while (runTIme < duration)
            {
                runTIme += Time.deltaTime;
                color.a = Mathf.Lerp(1.0f, fadeValue, runTIme / duration);
                blinkText.color = color;
                yield return null;
            }

            runTIme = 0.0f;
            color.a = 0.5f;
            blinkText.color = color;
            while (runTIme < duration)
            {
                runTIme += Time.deltaTime;
                color.a = Mathf.Lerp(fadeValue, 1.0f, runTIme / duration);
                blinkText.color = color;
                yield return null;
            }
        }

        textEffectLock = false;
        textEffectCoroutine = null;
    }
}
