using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ClearObject : UI_BaseObject
{
    [SerializeField] private Image fadeEffectImage;
    private Coroutine fadeEffectCoroutine = null;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Sound.StopBgm();

        if (fadeEffectCoroutine == null)
        {
            fadeEffectCoroutine = StartCoroutine(IfadeOutInEffect(2f));
        }

        return true;
    }

    private IEnumerator IfadeOutInEffect(float fadeTime)
    {
        Debug.Log("코루틴 시작");
        // FadeOut Effect
        fadeEffectImage.color = new Color(0, 0, 0, 1);
        Color tempColor = fadeEffectImage.color;

        while (tempColor.a > 0.01f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            fadeEffectImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 0f;
        fadeEffectImage.color = tempColor;
    }
}
