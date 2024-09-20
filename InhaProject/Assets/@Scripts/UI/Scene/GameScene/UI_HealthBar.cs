using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : UI_BaseObject
{
	[SerializeField] private Image hpSlider;
	[SerializeField] private Text hpText;

    float changeMoveSpeed = 1.5f;

    [SerializeField, ReadOnly] float maxHp = 0;
    [SerializeField, ReadOnly] float _currHp = 0;
    Coroutine coChangedHp = null;
    public float CurrHp
    {
        get { return _currHp; }
        protected set
        {
            _currHp = value;

            if (coChangedHp == null)
                coChangedHp = StartCoroutine(CoChangedHp());
        }
    }

	public override bool Init()
    {
		if (base.Init() == false)
			return false;

        return true;
    }

    public void SetInfo(float maxHp)
    {
        Managers.Game.Player.OnChangedHp -= OnChangedHp;
        Managers.Game.Player.OnChangedHp += OnChangedHp;

        this.maxHp = maxHp;
        CurrHp = maxHp;
        SetHpBar();
    }

    private void OnChangedHp(float currHp)
    {
        hpText.text = $"{CurrHp}/{maxHp}";
        this.CurrHp = currHp;
    }

    private void OnDisable()
    {
        if ( coChangedHp != null )
        {
            StopCoroutine(coChangedHp);
        }
    }

    private IEnumerator CoChangedHp()
    {
        while (Mathf.Abs(hpSlider.fillAmount - (CurrHp / maxHp)) > 0.01f)
        {
            hpSlider.fillAmount = Mathf.Lerp(hpSlider.fillAmount, CurrHp / maxHp, Time.deltaTime * changeMoveSpeed);
            yield return null;
        }

        SetHpBar();
        coChangedHp = null;
    }

    private void SetHpBar()
    {
        hpSlider.fillAmount = CurrHp / maxHp;
    }
}
