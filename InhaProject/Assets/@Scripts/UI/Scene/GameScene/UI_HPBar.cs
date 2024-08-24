using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : MonoBehaviour
{
	[SerializeField] private Image hpSlider;
	[SerializeField] private Text hpText;

	float playerHp = 100;
	float playerCurrentHp = 100;

	private void Start()
	{
		hpSlider.fillAmount = playerCurrentHp / playerHp;
		hpText.text = playerCurrentHp.ToString() + "/" + playerHp.ToString();
		// mpSlider.fillAmount = 
	}

	public void OnDamaged()
	{
		hpSlider.fillAmount = playerCurrentHp / playerHp;
		hpText.text = playerCurrentHp.ToString() + "/" + playerHp.ToString();
	}
}
