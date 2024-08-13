using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_IntroScene : UI_BaseScene
{
	[SerializeField] private TextMeshProUGUI blinkText;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}
}
