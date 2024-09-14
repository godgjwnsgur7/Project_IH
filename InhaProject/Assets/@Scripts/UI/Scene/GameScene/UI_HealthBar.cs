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
    [SerializeField, ReadOnly] float _currHp = 0;

    float changeMoveSpeed = 1.5f;
    Coroutine coChangedHp = null;
    public float currHp
    {
        get { return _currHp; }
        protected set
        {
            _currHp = value;
            if ( coChangedHp == null )
            {
                coChangedHp = StartCoroutine(CoChangedHp());
            }
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
        currHp = maxHp;
        SetHpBar();
    }

    private void OnChangedHp(float currHp)
    {
        this.currHp = currHp;
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
        hpText.text = $"{currHp}/{maxHp}";

        while ( hpSlider.fillAmount > currHp / maxHp + 0.01)
        {
            hpSlider.fillAmount = Mathf.Lerp(hpSlider.fillAmount, currHp / maxHp, Time.deltaTime * changeMoveSpeed);
            yield return null;
        }

        hpSlider.fillAmount = currHp / maxHp;
        coChangedHp = null;
    }

    private void SetHpBar()
    {
        hpSlider.fillAmount = currHp / maxHp;
    }
}
