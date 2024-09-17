using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : UI_BaseObject
{
	[SerializeField, ReadOnly] TextMeshProUGUI text;
	[SerializeField, ReadOnly] Image criticalImage;

	private float moveSpeed = 50.0f;
	private float alphaSpeed = 3.0f;
	private float destroyTime = 1.0f;
	private Vector3 monsterPos;

	private int damage;
	private bool bCritical;

	public override void SetInfo(UIParam param)
	{
		base.SetInfo(param);

		if (param is UIDamageParam uiDamageParam)
		{
			damage = uiDamageParam.damage;
			monsterPos = uiDamageParam.pos;
			text.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, monsterPos);

			text.text = damage.ToString();

			if (uiDamageParam.isCritical)
			{
				bCritical = true;
				criticalImage.enabled = true;
				text.color = Color.yellow;

				text.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, uiDamageParam.pos - new Vector3(criticalImage.flexibleWidth / 2, 0, 0));
				criticalImage.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, uiDamageParam.pos + new Vector3(criticalImage.flexibleWidth / 2, 0, 0));
			}
			else
			{
				criticalImage.enabled = false;
			}

			StartCoroutine(IfadeOutInDamage(destroyTime));
		}
	}

	private IEnumerator IfadeOutInDamage(float fadeTime)
    {
		Color textTempColor = text.color;

		while (textTempColor.a > 0f)
        {
			text.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, monsterPos);

			textTempColor.a -= Time.deltaTime / fadeTime;
			text.color = textTempColor;
			text.transform.Translate(new Vector3(0, Time.deltaTime * moveSpeed, 0));

			if ( bCritical )
			{
				float temp = Mathf.Lerp(text.transform.localScale.x, 1.5f, Time.deltaTime * alphaSpeed);
				
				Color imageTempColor = criticalImage.color;
				imageTempColor.a -= Time.deltaTime / fadeTime;
				criticalImage.color = imageTempColor;
				if ( text.color.a > 0.5f)
					criticalImage.transform.Translate(new Vector3(-(moveSpeed / 4 * Time.deltaTime), 0, 0));
				criticalImage.transform.Translate(new Vector3(0, Time.deltaTime * moveSpeed, 0));

				text.transform.localScale = new Vector3(temp, temp, 1);
				criticalImage.transform.localScale = new Vector3(temp, temp, 1);
			}

			yield return null;
        }

		DestroyObject();
    }

    private void Reset()
    {
        text = Util.FindChild<TextMeshProUGUI>(this.gameObject, "DamageText");
		criticalImage = Util.FindChild<Image>(this.gameObject, "CriticalImage");
    }

	private void DestroyObject()
	{
		Managers.Resource.Destroy(gameObject);
	}

}
