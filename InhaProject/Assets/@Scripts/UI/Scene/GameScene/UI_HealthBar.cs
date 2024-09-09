using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : UI_BaseObject
{
	[SerializeField] private Image hpSlider;
	[SerializeField] private Text hpText;

	[SerializeField, ReadOnly] float maxHp = 0;
    [SerializeField, ReadOnly] float currHp = 0;

	public override bool Init()
    {
		if (base.Init() == false)
			return false;

        Managers.Game.Player.OnChangedHp -= OnChangedHp;
        Managers.Game.Player.OnChangedHp += OnChangedHp;

        return true;
    }

    public void SetInfo(float maxHp)
    {
        this.maxHp = maxHp;
        currHp = maxHp;
        SetHpBar();
    }

    private void OnChangedHp(float currHp)
    {
		this.currHp = currHp;
        SetHpBar();
    }

    private void SetHpBar()
    {
        hpSlider.fillAmount = currHp / maxHp;
        hpText.text = $"{currHp}/{maxHp}";
    }
}
