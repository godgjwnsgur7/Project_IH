using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingPopup : UI_BasePopup
{
    [SerializeField] public Slider progressBar;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        progressBar.value = 0;

        return true;
    }

	private void Update()
	{
		progressBar.value += Time.deltaTime;
	}

	public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();


    }
}