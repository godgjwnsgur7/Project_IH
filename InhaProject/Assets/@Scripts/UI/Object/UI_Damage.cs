using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : UI_BaseObject
{
	public float moveSpeed = 100.0f;
	public float alphaSpeed = 3.0f;
	public float destroyTime = 2.0f;
	[SerializeField, ReadOnly] TextMeshProUGUI text;
	Color alpha;
	public int damage;
	Vector3 pos = new Vector3(0, 0, 0);


	// 코루틴으로 바꾸면서 없애주세용 ㅋㅋ tempColor만 있으면 될겁니다
	// -> FadeOutInPopup인가 걔 참고 ㄱㄱ
    private void Start()
	{
		alpha = text.color;
	}

    private void Reset()
    {
        text = Util.FindChild<TextMeshProUGUI>(this.gameObject, "DamageText");
    }

    // 코루틴으로 바꿔주세용 ㅎㅋ;;; 불편쓰
    private void Update()
	{
		text.text = damage.ToString();

		text.transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
		alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
		text.color = alpha;
	}

	public override void SetInfo(UIParam param)
	{
		base.SetInfo(param);

		if (param is UIDamageParam uiDamageParam)
		{
			damage = uiDamageParam.damage;
			text.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, uiDamageParam.pos);

			if (uiDamageParam.isCritical)
			{
				text.color = Color.yellow;
			}

            Invoke("DestroyObject", destroyTime); // 코루틴으로 빼주세영 ㅋ 끝나면 호출 ㅋㅋ
        }
	}

	private void DestroyObject()
	{
		Managers.Resource.Destroy(gameObject);
	}

}
