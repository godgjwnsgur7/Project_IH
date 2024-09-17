using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossMonsterStatus : UI_BaseObject
{
    [SerializeField] Image statusBarFiller;
    [SerializeField] TextMeshProUGUI monsterNameText;
    [SerializeField] TextMeshProUGUI statusBarText;
    float maxHp = 0;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        statusBarFiller.fillAmount = 1f;

        return true;
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        if(param is UIBossMonsterStatusParam bossMonsterStatusParam)
        {
            monsterNameText.text = bossMonsterStatusParam.monsterName;
            maxHp = bossMonsterStatusParam.maxHp;
            SetStatusBarText(maxHp, maxHp);
        }
    }

    private void SetStatusBarText(float maxHp, float currHp)
    {
        statusBarText.text = $"{currHp} / {maxHp}";
    }

    public void OnChangedCurrHp(float currHp)
    {
        if (currHp < 0)
            currHp = 0;

        SetStatusBarText(maxHp, currHp);
    }
}
