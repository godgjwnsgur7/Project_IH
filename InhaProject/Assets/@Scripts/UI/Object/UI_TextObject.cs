using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_TextObject : UI_BaseObject
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;
    private Vector3 pos;
    public float scaleSpeed = 3f;
    public float moveSpeed = 150f;
    float displayTime = 4f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false, 99);

        pos = text.rectTransform.position;
        pos.x -= 100;
        text.rectTransform.position = pos;
        pos.x += 100;
        text.rectTransform.localScale = new Vector3(0.3f, 0.3f, 1f);

        StartCoroutine("ITextMoveAndScale", 4);

        return true;
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        UITextParam test = param as UITextParam;

        if (test == null)
            return;

        if (test is UITextParam uiTextParam)
        {
            text.text = uiTextParam.text;
            displayTime = uiTextParam.displayTime;
        }

        StartCoroutine("ITextMoveAndScale", displayTime);
    }

	private IEnumerator ITextMoveAndScale(float displayTime)
	{
		Color textTempColor = text.color;
        textTempColor.a = 0f;
        float time = 0.0f;

        while ( time <= displayTime)
        {
            time += Time.deltaTime;
            float temp = Mathf.Lerp(text.transform.localScale.x, 1.5f, Time.deltaTime * scaleSpeed);

            text.color = textTempColor;
            textTempColor.a += Time.deltaTime;
            if ( text.rectTransform.position.x < pos.x )
			    text.transform.Translate(new Vector3(Time.deltaTime * moveSpeed, 0));

            if ( temp < 1f )
                text.transform.localScale = new Vector3(temp, temp, 1);
            yield return null;
		}

        textTempColor.a = 1f;
        text.color = textTempColor;
        StartCoroutine("IFadeOut", 1f);
	}

    private IEnumerator IFadeOut(float fadeTime)
    {
        Color textTemp = text.color;
        Color imageTempColor = image.color;

        while (textTemp.a > 0f)
        {
            textTemp.a -= Time.deltaTime / fadeTime;
            imageTempColor.a -= Time.deltaTime / fadeTime;

            text.color = textTemp;
            image.color = textTemp;

            yield return null;
        }

        Managers.Resource.Destroy(gameObject);
    }
}
