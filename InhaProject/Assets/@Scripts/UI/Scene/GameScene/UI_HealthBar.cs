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
		if (base.Init() == false)
			return false;

		return true;
	}

	private void OnChangedHp(float hp)
    {
		hpSlider.fillAmount = hp / player.PlayerInfo.MaxHp;
		hpText.text = hp.ToString() + "/" + player.PlayerInfo.MaxHp.ToString();
	}

	private void Start()
	{
		GameObject playerObject = GameObject.FindWithTag("Player");
		player = playerObject.GetComponent<Player>();

		if (player == null)
			return;

		hpSlider.fillAmount = player.PlayerInfo.CurrHp / player.PlayerInfo.MaxHp;
		hpText.text = player.PlayerInfo.CurrHp.ToString() + "/" + player.PlayerInfo.MaxHp.ToString();

		player.OnChangedHp += OnChangedHp;

		OnChangedHp(player.PlayerInfo.CurrHp);
	}
}
