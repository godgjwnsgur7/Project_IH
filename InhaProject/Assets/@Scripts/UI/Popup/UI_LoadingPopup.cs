using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingPopup : UI_BasePopup
{
    [SerializeField] public Slider progressBar;

    private void OnEnable()
    {
        coProgressBar = StartCoroutine(CoProgressBar());
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

	public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    Coroutine coProgressBar = null;
    protected IEnumerator CoProgressBar()
    {
        progressBar.value = 0f;

        while (progressBar.value < 0.99f)
        {
            progressBar.value += Time.deltaTime;
            yield return null;
        }

        progressBar.value = 1f;
        coProgressBar = null;
    }
}