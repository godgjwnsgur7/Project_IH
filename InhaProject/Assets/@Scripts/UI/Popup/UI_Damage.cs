using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : UI_BasePopup
{
	public float moveSpeed = 100.0f;
	public float alphaSpeed = 3.0f;
	public float destroyTime = 2.0f;
	TextMeshProUGUI text;
	Color alpha;
	public int damage;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}
	private void Start()
	{
		text = gameObject.GetComponent<TextMeshProUGUI>();
		alpha = text.color;
		Invoke("DestroyObject", destroyTime);
	}

	private void Update()
	{
		transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
		alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
		text.color = alpha;
	}

	public override void OpenPopupUI()
	{
		base.OpenPopupUI();
	}

	public override void SetInfo(UIParam param)
	{
		base.SetInfo(param);

		UIDamageParam test = param as UIDamageParam;

		if (test == null)
			return;

		if (test is UIDamageParam uiDamageParam)
		{
			
			//text.text = test.damage.ToString();
			Debug.Log("안되는듯한데");
			//text.transform.position = uiDamageParam.enermyPosition;

			//if ( uiDamageParam.isCritical )
			//{
			//	text.color = Color.yellow;
			//}
		}
	}

	public override void ClosePopupUI()
	{
		base.ClosePopupUI();
	}

	private void DestroyObject()
	{
		ClosePopupUI();
	}

}
