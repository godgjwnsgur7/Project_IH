using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterStatus : UI_BaseObject
{
    [field: SerializeField, ReadOnly] public BaseMonster Target { get; protected set; } = null;
    [SerializeField, ReadOnly] private RectTransform rectTr;
    [SerializeField, ReadOnly] private Slider hpBar;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        rectTr = GetComponent<RectTransform>();
        hpBar = GetComponent<Slider>();

        return true;
    }

    private void OnDisable()
    {
        if (coFollowTarget != null)
            StopCoroutine(coFollowTarget);
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        if(param is UIMonsterStatusParam monsterStatusParam && monsterStatusParam.monster != null)
        {
            if (coFollowTarget != null)
                StopCoroutine(coFollowTarget);

            Target = monsterStatusParam.monster;

            float rectSizeX = Target.GetSizeX() * 100;
            rectTr.sizeDelta = new Vector2(rectSizeX, rectSizeX * 0.15f);
            Target.OnChangedCurrHp -= OnChangedCurrHp;
            Target.OnChangedCurrHp += OnChangedCurrHp;

            coFollowTarget = StartCoroutine(CoFollowTarget());
        }
    }

    public void OnChangedCurrHp(float currHp)
    {
        hpBar.value = currHp / Target.GetMaxHp();
    }

    Coroutine coFollowTarget = null;
    protected IEnumerator CoFollowTarget()
    {
        while(Target != null)
        {
            rectTr.transform.position = Camera.main.WorldToScreenPoint(Target.GetTopPosition());

            yield return new LateUpdate();
        }

        Target = null;
        coFollowTarget = null;
        Managers.Resource.Destroy(this.gameObject);
    }
}