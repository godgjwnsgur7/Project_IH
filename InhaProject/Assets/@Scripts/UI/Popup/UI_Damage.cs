using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : MonoBehaviour
{
	public float moveSpeed = 100.0f;
	public float alphaSpeed = 3.0f;
	public float destroyTime = 2.0f;
	TextMeshProUGUI text;
	Color alpha;
	public int damage;
	Vector3 position = new Vector3(0, 0, 0);
	Transform childTransform;


	private void Start()
	{
		childTransform = transform.Find("DamageText");
		text = childTransform.GetComponent<TextMeshProUGUI>();

		alpha = text.color;
		Invoke("DestroyObject", destroyTime);
	}

	private void Update()
	{
		text.text = damage.ToString();

		text.transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
		alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
		text.color = alpha;
	}

	public void SetInfo(UIParam param)
	{
		//base.SetInfo(param);

		UIDamageParam test = param as UIDamageParam;

		if (test == null)
			return;

		
		if (test is UIDamageParam uiDamageParam)
		{
			if ( text == null )
			{
				childTransform = transform.Find("DamageText");
				text = childTransform.GetComponent<TextMeshProUGUI>();
			}

			damage = uiDamageParam.damage;
			position = uiDamageParam.enermyPosition;
			text.transform.position = Camera.main.WorldToScreenPoint(position);

			if (uiDamageParam.isCritical)
			{
				text.color = Color.yellow;
			}
		}
	}

	private void DestroyObject()
	{
		Destroy(gameObject);
	}

}
