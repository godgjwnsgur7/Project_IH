using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : UI_GameScene
{
	[SerializeField] private Image hpSlider;
	[SerializeField] private Text hpText;

	private Player player;
	public override bool Init()
	{
		return true;
	}

	private void Awake()
	{
		GameObject playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null)
		{
			player = playerObject.GetComponent<Player>();
			player.OnChangedHp += OnChangedHp;
		}
	}

	private void Start()
	{
		StartCoroutine(SetPlayerInfo());
	}

	private void OnChangedHp(float hp)
    {
		hpSlider.fillAmount = hp / player.PlayerInfo.MaxHp;
		hpText.text = hp.ToString() + "/" + player.PlayerInfo.MaxHp.ToString();
	}

	IEnumerator SetPlayerInfo()
	{
		yield return new WaitForFixedUpdate();
		OnChangedHp(player.PlayerInfo.MaxMp);
	}
}
