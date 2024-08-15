using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffectPopup : InitBase
{
    [SerializeField] private Image fadeEffectImage;

    private Coroutine fadeEffectCoroutine = null;
    private Func<bool> fadeInEffectCondition = null;
    private Action onFadeOutCallBack = null;
    private Action onFadeInCallBack = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void OnDisable()
    {
        if (fadeEffectCoroutine != null)
            return;

        fadeInEffectCondition = null;
        onFadeOutCallBack = null;
        onFadeInCallBack = null;
    }

    public void PlayFadeOutInEffect(UIParam param = null)
    {
        if (param is UIFadeEffectParam fadeEffectParam)
        {
            if (fadeEffectCoroutine == null)
            {
                onFadeOutCallBack = fadeEffectParam.onFadeOutCallBack;
                onFadeInCallBack = fadeEffectParam.onFadeInCallBack;
                fadeEffectCoroutine = StartCoroutine(IfadeOutInEffect(0.5f));
            }
            else
                Debug.Log("FadeEffect 중복");
        }

        gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator IfadeOutInEffect(float fadeTime)
    {
        // FadeOut Effect
        fadeEffectImage.color = new Color(0, 0, 0, 0);
        Color tempColor = fadeEffectImage.color;

        while (tempColor.a < 0.99f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            fadeEffectImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 1f;
        fadeEffectImage.color = tempColor;
        onFadeOutCallBack?.Invoke();

        // Wait Condition
        if (fadeInEffectCondition != null)
            yield return new WaitUntil(fadeInEffectCondition);

        // FadeIn Effect
        while (tempColor.a > 0.01f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            fadeEffectImage.color = tempColor;
            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        onFadeInCallBack?.Invoke();

        fadeEffectImage.color = tempColor;
        fadeEffectCoroutine = null;
        ClosePopup();
    }
}