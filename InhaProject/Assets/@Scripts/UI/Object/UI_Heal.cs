using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Heal : UI_BaseObject
{
    [SerializeField, ReadOnly] TextMeshProUGUI healText;
    [SerializeField, ReadOnly] TextMeshProUGUI manaText;
    private TextMeshProUGUI text;

    private float moveSpeed = 50.0f;
    private float alphaSpeed = 3.0f;
    private float destroyTime = 1.0f;

    private int healAmount;
    private bool isHp;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        healText.enabled = false;
        manaText.enabled = false;

        return true;
    }
    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        if (param is UIHealParam uiHealParam)
        {
            healAmount = uiHealParam.healAmount;
            isHp = uiHealParam.isHp;

            if ( isHp )
            {
                healText.text = healAmount.ToString();
                healText.enabled = true;
                text = healText;
            }
            else
            {
                manaText.text = healAmount.ToString();
                manaText.enabled = true;
                text = manaText;
            }

            StartCoroutine(IfadeOutInDamage(destroyTime));
        }
    }

    private IEnumerator IfadeOutInDamage(float fadeTime)
    {
        Color textTempColor = text.color;

        while (textTempColor.a > 0f)
        {
            textTempColor.a -= Time.deltaTime / fadeTime;
            text.color = textTempColor;
            text.transform.Translate(new Vector3(0, Time.deltaTime * moveSpeed, 0));

            float temp = Mathf.Lerp(text.transform.localScale.x, 1.5f, Time.deltaTime * alphaSpeed);
            text.transform.localScale = new Vector3(temp, temp, 1);
            
            yield return null;
        }

        DestroyObject();
    }
    private void Reset()
    {
        healText = Util.FindChild<TextMeshProUGUI>(this.gameObject, "HealText");
        manaText = Util.FindChild<TextMeshProUGUI>(this.gameObject, "ManaText");
    }
    
    private void DestroyObject()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
