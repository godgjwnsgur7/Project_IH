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


	// �ڷ�ƾ���� �ٲٸ鼭 �����ּ��� ���� tempColor�� ������ �ɰ̴ϴ�
	// -> FadeOutInPopup�ΰ� �� ���� ����
    private void Start()
	{
		alpha = text.color;
	}

    private void Reset()
    {
        text = Util.FindChild<TextMeshProUGUI>(this.gameObject, "DamageText");
    }

    // �ڷ�ƾ���� �ٲ��ּ��� ����;;; ����
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

            Invoke("DestroyObject", destroyTime); // �ڷ�ƾ���� ���ּ��� �� ������ ȣ�� ����
        }
	}

	private void DestroyObject()
	{
		Managers.Resource.Destroy(gameObject);
	}

}
